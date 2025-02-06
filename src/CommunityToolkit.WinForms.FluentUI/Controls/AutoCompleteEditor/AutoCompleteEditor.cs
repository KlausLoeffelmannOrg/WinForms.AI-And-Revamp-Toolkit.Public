using CommunityToolkit.WinForms.AsyncSupport;
using System.ComponentModel;
using Timer = System.Windows.Forms.Timer;

namespace CommunityToolkit.WinForms.FluentUI.Controls;

/// <summary>
///  A RichTextBox control with auto-complete and suggestion capabilities.
/// </summary>
/// <remarks>
///  <para>
///   This control extends the RichTextBox to provide auto-complete suggestions and command
///   sending functionality. It includes various properties to customize the behavior and
///   appearance of the suggestions.
///  </para>
///  <para>
///   The control uses a timer to manage the timing of suggestion requests and can display
///   an overlay panel to indicate waiting states.
///  </para>
/// </remarks>
public partial class AutoCompleteEditor : RichTextBox
{
    /// <summary>
    ///  Event triggered to request auto-complete suggestions asynchronously.
    /// </summary>
    public event AsyncEventHandler<AsyncRequestAutoCompleteSuggestionEventArgs>? AsyncRequestAutoCompleteSuggestion;

    /// <summary>
    ///  Event triggered to send commands asynchronously.
    /// </summary>
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

    /// <summary>
    ///  Initializes a new instance of the <see cref="AutoCompleteEditor"/> class.
    /// </summary>
    public AutoCompleteEditor()
    {
        _semanticKernel = new()
        {
            ModelId = "gpt-3.5-turbo",
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

    /// <summary>
    ///  Gets or sets the minimum character count to trigger suggestion request.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property defines the minimum number of characters that must be typed before
    ///   a suggestion request is triggered.
    ///  </para>
    /// </remarks>
    [Description("Minimum character count to trigger suggestion request.")]
    [DefaultValue(5)]
    [Category("AI: prediction behavior")]
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

    /// <summary>
    ///  Gets or sets the maximum character count to trigger suggestion request.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property defines the maximum number of characters that can remain before
    ///   a suggestion request is triggered.
    ///  </para>
    /// </remarks>
    [Description("Maximum character count to trigger suggestion request.")]
    [DefaultValue(10)]
    [Category("AI: prediction behavior")]
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

    /// <summary>
    ///  Gets or sets the minimum character changes before the next text review request.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property defines the minimum number of character changes that must occur
    ///   before the next text review request is triggered.
    ///  </para>
    /// </remarks>
    [DefaultValue(50)]
    [Category("AI: prediction behavior")]
    public int MinCharChangedBeforeNextTextReviewRequest
    {
        get => _minCharChangedBeforeNextTextReviewRequest;
        set => _minCharChangedBeforeNextTextReviewRequest = value;
    }

    /// <summary>
    ///  Gets or sets the minimum time threshold for the next text review request.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property defines the minimum time in seconds that must pass before the next
    ///   text review request is triggered.
    ///  </para>
    /// </remarks>
    [DefaultValue(2f)]
    [Category("AI: prediction behavior")]
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

    /// <summary>
    ///  Gets or sets the minimum time in seconds to trigger suggestion request.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property defines the minimum time in seconds that must pass before a
    ///   suggestion request is triggered.
    ///  </para>
    /// </remarks>
    [Description("Minimum time in seconds to trigger suggestion request.")]
    [DefaultValue(0f)]
    [Category("AI: prediction behavior")]
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

    /// <summary>
    ///  Gets or sets a value indicating whether to allow replacement of existing remaining text.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property determines whether the existing remaining text can be replaced by
    ///   the auto-complete suggestions.
    ///  </para>
    /// </remarks>
    [Description("Allow replacement of existing remaining text.")]
    [DefaultValue(false)]
    [Category("AI: prediction behavior")]
    public bool AllowExistingRemainingTextReplacement
    {
        get => _allowExistingRemainingTextReplacement;
        set => _allowExistingRemainingTextReplacement = value;
    }

    /// <summary>
    ///  Gets or sets the color for standard text editing.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property defines the color used for standard text editing in the control.
    ///  </para>
    /// </remarks>
    [Description("Color for standard text editing.")]
    [Category("AI: Appearance")]
    public Color StandardEditColor
    {
        get => _standardEditColor;
        set => _standardEditColor = value;
    }

    /// <summary>
    ///  Gets or sets the color for suggestions.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property defines the color used for displaying suggestions in the control.
    ///  </para>
    /// </remarks>
    [Description("Color for suggestions.")]
    [Category("AI: Appearance")]
    public Color SuggestionColor
    {
        get => _suggestionColor;
        set => _suggestionColor = value;
    }

    /// <summary>
    ///  Gets or sets the color for auto-corrections.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property defines the color used for displaying auto-corrections in the control.
    ///  </para>
    /// </remarks>
    [Description("Color for auto-corrections.")]
    [Category("AI: Appearance")]
    public Color AutoCorrectionColor
    {
        get => _autoCorrectionColor;
        set => _autoCorrectionColor = value;
    }

    /// <summary>
    ///  Gets or sets the color for correction candidates.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This property defines the color used for displaying correction candidates in the
    ///   control.
    ///  </para>
    /// </remarks>
    [Description("Color for correction candidates.")]
    [Category("AI: Appearance")]
    public Color CorrectionCandidateColor
    {
        get => _correctionCandidateColor;
        set => _correctionCandidateColor = value;
    }

    /// <summary>
    ///  Determines whether the standard edit color should be serialized.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the standard edit color should be serialized; otherwise, <c>false</c>.
    /// </returns>
    private bool ShouldSerializeStandardEditColor()
    {
        return Application.IsDarkModeEnabled
            ? _standardEditColor != DefaultDarkModeStandardEditColor
            : _standardEditColor != DefaultLightModeStandardEditColor;
    }

    /// <summary>
    ///  Determines whether the suggestion color should be serialized.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the suggestion color should be serialized; otherwise, <c>false</c>.
    /// </returns>
    private bool ShouldSerializeSuggestionColor()
    {
        return Application.IsDarkModeEnabled
            ? _suggestionColor != DefaultDarkModeSuggestionColor
            : _suggestionColor != DefaultLightModeSuggestionColor;
    }

    /// <summary>
    ///  Determines whether the auto-correction color should be serialized.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the auto-correction color should be serialized; otherwise, <c>false</c>.
    /// </returns>
    private bool ShouldSerializeAutoCorrectionColor()
    {
        return Application.IsDarkModeEnabled
            ? _autoCorrectionColor != DefaultDarkModeAutoCorrectionColor
            : _autoCorrectionColor != DefaultLightModeAutoCorrectionColor;
    }

    /// <summary>
    ///  Determines whether the correction candidate color should be serialized.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the correction candidate color should be serialized; otherwise, <c>false</c>.
    /// </returns>
    private bool ShouldSerializeCorrectionCandidateColor()
    {
        return Application.IsDarkModeEnabled
            ? _correctionCandidateColor != DefaultDarkModeCorrectionCandidateColor
            : _correctionCandidateColor != DefaultLightModeCorrectionCandidateColor;
    }

    /// <summary>
    ///  Raises the <see cref="AsyncRequestAutoCompleteSuggestion"/> event.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected virtual void OnRequestAutoCompleteSuggestion(AsyncRequestAutoCompleteSuggestionEventArgs e)
    {
        AsyncRequestAutoCompleteSuggestion?.Invoke(this, e);
    }

    /// <summary>
    ///  Raises the <see cref="Control.ReadOnlyChanged"/> event.
    /// </summary>
    /// <param name="e">The event data.</param>
    /// <remarks>
    ///  <para>
    ///   This method updates the background and foreground colors of the control based on
    ///   the read-only state and the application's dark mode setting.
    ///  </para>
    /// </remarks>
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

    /// <summary>
    ///  Initializes the overlay panel.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This method sets up the overlay panel that is used to indicate waiting states.
    ///   The overlay panel is resized to match the size of the RichTextBox and is added
    ///   to the control's collection of child controls.
    ///  </para>
    /// </remarks>
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

    /// <summary>
    ///  Ensures that the overlay panel is initialized.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This method checks if the overlay panel is null and initializes it if necessary.
    ///  </para>
    /// </remarks>
    private void EnsureOverlayInitialized()
    {
        if (_overlayPanel is null)
        {
            InitializeOverlay();
        }
    }

    /// <summary>
    ///  Shows the overlay panel.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This method makes the overlay panel visible and freezes the content of the
    ///   RichTextBox.
    ///  </para>
    /// </remarks>
    public void ShowOverlay()
    {
        EnsureOverlayInitialized();
        _overlayPanel.FreezeContent(this);
        _overlayPanel.Visible = true;
    }

    /// <summary>
    ///  Hides the overlay panel.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This method hides the overlay panel.
    ///  </para>
    /// </remarks>
    public void HideOverlay()
    {
        EnsureOverlayInitialized();
        _overlayPanel.Visible = false;
    }
}
