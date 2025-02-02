using CommunityToolkit.WinForms.AsyncSupport;
using System.ComponentModel;

using Timer = System.Windows.Forms.Timer;

namespace CommunityToolkit.WinForms.FluentUI.Controls;

public partial class AutoCompleteEditor : RichTextBox
{
    public event AsyncEventHandler<AsyncRequestAutoCompleteSuggestionEventArgs>? AsyncRequestAutoCompleteSuggestion;
    public event AsyncEventHandler<AsyncSendCommandEventArgs>? AsyncSendCommand;

    private int _minCharsSuggestionRequestSensitivity = 5;
    private int _maxRemainingCharsSuggestionRequestSensitivity = 0;
    private float _minTimeSuggestionRequestSensitivity = 0;
    private int _minCharChangedBeforeNextTextReviewRequest = 60;
    private float _minTimeThresholdForNextTextReviewRequest = 4;

    private bool _allowExistingRemainingTextReplacement;
    private Color _standardEditColor;
    private Color _suggestionColor;
    private Color _autoCorrectionColor;
    private Color _correctionCandidateColor;

    private readonly Timer _suggestionTimer;
    private TextSpan _currentSuggestion;
    private string _oldParagraph = string.Empty;

    private static readonly Color DefaultLightModeStandardEditColor = Color.Black;
    private static readonly Color DefaultDarkModeStandardEditColor = Color.White;
    private static readonly Color DefaultLightModeSuggestionColor = Color.Blue;
    private static readonly Color DefaultDarkModeSuggestionColor = Color.Gray;
    private static readonly Color DefaultLightModeAutoCorrectionColor = Color.Green;
    private static readonly Color DefaultDarkModeAutoCorrectionColor = Color.LightGreen;
    private static readonly Color DefaultLightModeCorrectionCandidateColor = Color.Red;
    private static readonly Color DefaultDarkModeCorrectionCandidateColor = Color.OrangeRed;
    private ContentFreezePanel _overlayPanel = null!;

    // Call InitializeOverlay in the constructor
    public AutoCompleteEditor()
    {
        _semanticKernel = new()
        {
            ModelName = "gpt-3.5-turbo",
            MaxTokens = 4096
        };

        _suggestionTimer = new Timer();
        _suggestionTimer.Tick += SuggestionTimer_Tick;

        if (Application.IsDarkModeEnabled)
        {
            _standardEditColor = DefaultDarkModeStandardEditColor;
            _suggestionColor = DefaultDarkModeSuggestionColor;
            _autoCorrectionColor = DefaultDarkModeAutoCorrectionColor;
            _correctionCandidateColor = DefaultDarkModeCorrectionCandidateColor;
        }
        else
        {
            _standardEditColor = DefaultLightModeStandardEditColor;
            _suggestionColor = DefaultLightModeSuggestionColor;
            _autoCorrectionColor = DefaultLightModeAutoCorrectionColor;
            _correctionCandidateColor = DefaultLightModeCorrectionCandidateColor;
        }

        InitializeOverlay();
    }

    [Description("Minimum character count to trigger suggestion request.")]
    [DefaultValue(5)]
    [Category("Behavior")]
    public int MinPrecedingCharsSuggestionRequestSensitivity
    {
        get => _minCharsSuggestionRequestSensitivity;
        set
        {
            if (value < 5 || value > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Valid values are between 5 and 20.");
            }

            _minCharsSuggestionRequestSensitivity = value;
        }
    }

    [Description("Maximum character count to trigger suggestion request.")]
    [DefaultValue(10)]
    [Category("Behavior")]
    public int MaxRemainingCharsSuggestionRequestSensitivity
    {
        get => _maxRemainingCharsSuggestionRequestSensitivity;
        set
        {
            if (value < 0 || value > 200)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Valid values are between 3 and 200.");
            }

            _maxRemainingCharsSuggestionRequestSensitivity = value;
        }
    }

    [DefaultValue(50)]
    public int MinCharChangedBeforeNextTextReviewRequest
    {
        get => _minCharChangedBeforeNextTextReviewRequest;
        set => _minCharChangedBeforeNextTextReviewRequest = value;
    }

    [DefaultValue(2f)]
    public float MinTimeThresholdForNextTextReviewRequest
    {
        get => _minTimeThresholdForNextTextReviewRequest;
        set
        {
            if (_minTimeThresholdForNextTextReviewRequest == value)
            {
                return;
            }

            if (value < 2 || value > 30)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Valid values are between 0 and 9 seconds.");
            }

            bool wasOff = _minTimeThresholdForNextTextReviewRequest == 0;
            _minTimeThresholdForNextTextReviewRequest = value;

            if (wasOff)
            {
                StartOrResetSuggestionTimer();
            }

            if (value == 0)
            {
                _suggestionTimer.Stop();
            }
            else
            {
                _suggestionTimer.Interval = (int)(value * 1000);
            }
        }
    }

    [Description("Minimum time in seconds to trigger suggestion request.")]
    [DefaultValue(0f)]
    [Category("Behavior")]
    public float MinTimeSuggestionRequestSensitivity
    {
        get => _minTimeSuggestionRequestSensitivity;

        set
        {
            if (_minTimeSuggestionRequestSensitivity == value)
            {
                return;
            }

            if (value < 0 || value > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Valid values are between 0 and 9 seconds.");
            }

            bool wasOff = _minTimeSuggestionRequestSensitivity == 0;
            _minTimeSuggestionRequestSensitivity = value;

            if (wasOff)
            {
                StartOrResetSuggestionTimer();
            }

            if (value == 0)
            {
                _suggestionTimer.Stop();
            }
            else
            {
                _suggestionTimer.Interval = (int)(value * 1000);
            }
        }
    }

    [Description("Allow replacement of existing remaining text.")]
    [DefaultValue(false)]
    [Category("Behavior")]
    public bool AllowExistingRemainingTextReplacement
    {
        get => _allowExistingRemainingTextReplacement;
        set => _allowExistingRemainingTextReplacement = value;
    }

    [Description("Color for standard text editing.")]
    [Category("Appearance")]
    public Color StandardEditColor
    {
        get => _standardEditColor;
        set => _standardEditColor = value;
    }

    [Description("Color for suggestions.")]
    [Category("Appearance")]
    public Color SuggestionColor
    {
        get => _suggestionColor;
        set => _suggestionColor = value;
    }

    [Description("Color for auto-corrections.")]
    [Category("Appearance")]
    public Color AutoCorrectionColor
    {
        get => _autoCorrectionColor;
        set => _autoCorrectionColor = value;
    }

    [Description("Color for correction candidates.")]
    [Category("Appearance")]
    public Color CorrectionCandidateColor
    {
        get => _correctionCandidateColor;
        set => _correctionCandidateColor = value;
    }

    private bool ShouldSerializeStandardEditColor()
    {
        return Application.IsDarkModeEnabled
            ? _standardEditColor != DefaultDarkModeStandardEditColor
            : _standardEditColor != DefaultLightModeStandardEditColor;
    }

    private bool ShouldSerializeSuggestionColor()
    {
        return Application.IsDarkModeEnabled
            ? _suggestionColor != DefaultDarkModeSuggestionColor
            : _suggestionColor != DefaultLightModeSuggestionColor;
    }

    private bool ShouldSerializeAutoCorrectionColor()
    {
        return Application.IsDarkModeEnabled
            ? _autoCorrectionColor != DefaultDarkModeAutoCorrectionColor
            : _autoCorrectionColor != DefaultLightModeAutoCorrectionColor;
    }

    private bool ShouldSerializeCorrectionCandidateColor()
    {
        return Application.IsDarkModeEnabled
            ? _correctionCandidateColor != DefaultDarkModeCorrectionCandidateColor
            : _correctionCandidateColor != DefaultLightModeCorrectionCandidateColor;
    }

    protected virtual void OnRequestAutoCompleteSuggestion(AsyncRequestAutoCompleteSuggestionEventArgs e)
    {
        AsyncRequestAutoCompleteSuggestion?.Invoke(this, e);
    }

    protected override void OnReadOnlyChanged(EventArgs e)
    {
        base.OnReadOnlyChanged(e);

        if (ReadOnly)
        {
            if (Application.IsDarkModeEnabled)
            {
                this.BackColor = Color.FromArgb(30, 30, 30); // Dark mode background color for read-only
                this.ForeColor = Color.LightGray; // Dark mode text color for read-only
            }
            else
            {
                this.BackColor = Color.LightGray; // Light mode background color for read-only
                this.ForeColor = Color.DarkGray; // Light mode text color for read-only
            }
        }
        else
        {
            if (Application.IsDarkModeEnabled)
            {
                this.BackColor = Color.Black; // Dark mode background color for editable
                this.ForeColor = Color.White; // Dark mode text color for editable
            }
            else
            {
                this.BackColor = Color.White; // Light mode background color for editable
                this.ForeColor = Color.Black; // Light mode text color for editable
            }
        }
    }

    private void InitializeOverlay()
    {
        _overlayPanel = new ContentFreezePanel
        {
            Visible = false,
            Text = "Waiting...", // Text rendered by the control.
            Font = new Font(Font.FontFamily, Font.Size * 2, FontStyle.Bold),
            ForeColor = Color.White,
            // Resize the overlay panel to match the size of the RichTextBox
            Size = this.Size
        };

        // Add event handler to resize the overlay panel when the RichTextBox is resized
        this.SizeChanged += (sender, e) => _overlayPanel.Size = this.Size;

        Controls.Add(_overlayPanel);
    }

    private void EnsureOverlayInitialized()
    {
        if (_overlayPanel is null)
        {
            InitializeOverlay();
        }
    }

    public void ShowOverlay()
    {
        EnsureOverlayInitialized();
        _overlayPanel.FreezeContent(this);
        _overlayPanel.Visible = true;
    }

    public void HideOverlay()
    {
        EnsureOverlayInitialized();
        _overlayPanel.Visible = false;
    }
}
