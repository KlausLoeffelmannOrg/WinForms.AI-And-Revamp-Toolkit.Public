using CommunityToolkit.WinForms.AI.SemanticKernel;
using CommunityToolkit.WinForms.AsyncSupport;

namespace CommunityToolkit.WinForms.FluentUI.Controls;

public partial class AutoCompleteEditor : RichTextBox, IUsesSemanticKernelComponent
{
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.KeyCode == Keys.Escape)
        {
            RejectSuggestion();
        }
        else if (e.KeyCode == Keys.Enter && !e.Control && !e.Alt && !e.Shift)
        {
            e.SuppressKeyPress = true; // Prevent the Enter key from being input as text
            RejectSuggestion();
            SendCommand();
        }
        else if (e.KeyCode == Keys.Right && e.Control && IsCursorAtEnd)
        {
            e.SuppressKeyPress = true; // Prevent the Tab key from being input as text
            AcceptSuggestionWordWise();
        }
        // We can only accept suggestions if the control key is pressed,
        // because Focus-Handling!
        else if (e.KeyCode == Keys.Right && IsCursorAtEnd)
        {
            e.SuppressKeyPress = true; // Prevent the Tab key from being input as text
            AcceptSuggestion();
        }
        else if (e.KeyCode == Keys.Space && e.Control)
        {
            e.SuppressKeyPress = true; // Prevent the Space key from being input as text
            RejectSuggestion();
            ForceNewSuggestion();
        }
        else
        {
            // Only pressing one of the control keys does not reject BUT restart the suggestion timer:
            if (e.KeyCode is Keys.ControlKey or Keys.Menu or Keys.ShiftKey)
            {
                return;
            }

            RejectSuggestion();
            StartOrResetSuggestionTimer();
        }
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        if (e.KeyCode != Keys.Tab && e.KeyCode != Keys.Right && !e.Control)
        {
            RejectSuggestion();
        }
    }

    private void SendCommand()
    {
        var eArgs = new AsyncSendCommandEventArgs(
            this.Text,
            this.Rtf);

        _oldParagraph = CurrentParagraphSpan.ToString();
        AsyncExtensions.RunAsync(this, OnSendCommandAsync, eArgs);
    }

    public Task SendCommandAsync()
    {
        var eArgs = new AsyncSendCommandEventArgs(
            this.Text,
            this.Rtf);

        _oldParagraph = CurrentParagraphSpan.ToString();
        return OnSendCommandAsync(eArgs);
    }

    private void RejectSuggestion()
    {
        if (_currentSuggestion.IsEmpty)
        {
            return;
        }

        SelectionStart = _currentSuggestion.StartIncludingSymbol;
        SelectionLength = _currentSuggestion.LengthIncludingSymbol;

        // We delete the suggestion text and the TabSymbol
        SelectedText = string.Empty;
        _currentSuggestion = default;
    }

    protected virtual async Task OnSendCommandAsync(AsyncSendCommandEventArgs eArgs)
    {
        if (AsyncSendCommand is null)
        {
            return;
        }

        AsyncEventInvoker<AsyncSendCommandEventArgs> invoker = new(
            asyncEventHandler: AsyncSendCommand,
            parallelInvoke: false);

        ShowOverlay();
        await invoker.RaiseEventAsync(this, eArgs);
        HideOverlay();
    }

    private void StartOrResetSuggestionTimer()
    {
        if (MinTimeSuggestionRequestSensitivity == 0)
        {
            return;
        }

        if (_suggestionTimer.Enabled)
        {
            _suggestionTimer.Stop();
        }

        _suggestionTimer.Start();
    }

    private void SuggestionTimer_Tick(object? sender, EventArgs e)
    {
        _suggestionTimer.Stop();
        RequestSuggestion();
    }

    private void RequestSuggestion()
    {
        if (PrecedingParagraphSpan.Length >= MinPrecedingCharsSuggestionRequestSensitivity 
            && RemainingParagraphSpan.Length <= MaxRemainingCharsSuggestionRequestSensitivity)
        {
            var args = new AsyncRequestAutoCompleteSuggestionEventArgs(
                currentParagraph: CurrentParagraphSpan.ToString(),
                oldParagraph: OldParagraph,
                isCursorAtEnd: IsCursorAtEnd,
                cursorLocation: CursorLocation,
                suggestionText: string.Empty);

            OnRequestAutoCompleteSuggestion(args);
            ShowSuggestion(args.SuggestionText);
        }
    }

    /// <summary>
    /// Inserts the EffectiveSymbolText with only the symbol part formatted as the symbol font,
    /// superscripted, and using SuggestionColor, while the leading space uses the normal formatting.
    /// Then inserts the suggestion text using SuggestionColor and the normal font, preserving the original selection.
    /// </summary>
    /// <param name="suggestionText">The text of the suggestion to insert.</param>
    private void ShowSuggestion(string suggestionText)
    {
        if (string.IsNullOrEmpty(suggestionText))
        {
            return;
        }

        int originalSelectionStart = SelectionStart;
        int originalSelectionLength = SelectionLength;

        Font fontInUse = GetCurrentCursorFont();
        Font symbolFont = GetDerivedCachedSymbolFont();

        // EffectiveSymbolText is defined as " " + SymbolText.
        string effectiveSymbolText = TextSpan.EffectiveSymbolText;
        // The first character (a space) should NOT get symbol formatting.
        string firstChar = effectiveSymbolText[..1];
        // The remainder (symbol + trailing space) should be formatted.
        string symbolPart = effectiveSymbolText[1..];

        // Insert the first character using normal formatting (standard color and font).
        SelectionColor = StandardEditColor;
        SelectionFont = fontInUse;
        SelectedText = firstChar;

        // Insert the symbol part using SuggestionColor and the symbol font.
        SelectionColor = SuggestionColor;
        SelectionFont = symbolFont;
        SelectedText = symbolPart;

        // Apply superscript formatting to the symbol part.
        int symbolPartLength = symbolPart.Length;
        // Select the symbol part (starting after the first char).
        SelectionStart = originalSelectionStart + 1;
        SelectionLength = symbolPartLength;
        SelectionCharOffset = 4; // Superscript effect.

        // Move caret to the end of EffectiveSymbolText and reset formatting.
        SelectionStart = originalSelectionStart + effectiveSymbolText.Length;
        SelectionLength = 0;
        SelectionCharOffset = 0;

        // Insert the suggestion text using SuggestionColor and normal font.
        SelectionColor = SuggestionColor;
        SelectionFont = fontInUse;
        SelectedText = suggestionText;

        // Restore the original selection.
        SelectionStart = originalSelectionStart;
        SelectionLength = originalSelectionLength;

        _currentSuggestion = new(
            originalSelectionStart + TextSpan.SymbolLength,
            suggestionText.Length,
            suggestionText.AsMemory());
    }

    private TextSpan SelectionSpan => new(
                SelectionStart,
                SelectionLength,
                SelectedText.AsMemory());

    private Font GetCurrentCursorFont()
        => SelectionFont ?? Font;

    private Font GetDerivedCachedSymbolFont() 
        => new(
            "Segoe UI Symbol",
            GetCurrentCursorFont().Size + 4,
            FontStyle.Bold);

    /// <summary>
    ///  Gets the current paragraph as a ReadOnlySpan&lt;char&gt; 
    ///  based on the caret position.
    /// </summary>
    private ReadOnlySpan<char> CurrentParagraphSpan
    {
        get
        {
            ReadOnlySpan<char> textSpan = Text.AsSpan();
            int start = SelectionStart > 0
                ? textSpan[..SelectionStart].LastIndexOf('\n') + 1
                : 0;

            int newlineOffset = textSpan[SelectionStart..].IndexOf('\n');

            int end = newlineOffset is -1 
                ? textSpan.Length 
                : SelectionStart + newlineOffset;
            
            return textSpan[start..end];
        }
    }

    /// <summary>
    ///  Gets the remaining paragraph text as a ReadOnlySpan&lt;char&gt; 
    ///  starting at the caret position.
    /// </summary>
    private ReadOnlySpan<char> RemainingParagraphSpan
    {
        get
        {
            ReadOnlySpan<char> textSpan = Text.AsSpan();
            int newlineOffset = textSpan[SelectionStart..].IndexOf('\n');

            int end = newlineOffset is -1 
                ? textSpan.Length 
                : SelectionStart + newlineOffset;
            
            return textSpan[SelectionStart..end];
        }
    }

    /// <summary>
    ///  Gets the preceding paragraph text as a ReadOnlySpan&lt;char&gt; 
    ///  before the caret position.
    /// </summary>
    private ReadOnlySpan<char> PrecedingParagraphSpan
    {
        get
        {
            ReadOnlySpan<char> textSpan = Text.AsSpan();
            int start = textSpan[..SelectionStart].LastIndexOf('\n') + 1;
            return textSpan[start..SelectionStart];
        }
    }

    private void AcceptSuggestion()
    {
        if (!_currentSuggestion.IsEmpty)
        {
            SelectionStart = _currentSuggestion.StartIncludingSymbol;
            SelectionLength = _currentSuggestion.LengthIncludingSymbol;
            SelectedText = string.Empty;
            SelectedText = _currentSuggestion.Text.ToString();
            _currentSuggestion = default;
        }
    }

    private void AcceptSuggestionWordWise()
    {
        if (!_currentSuggestion.IsEmpty)
        {
            // Find the next word boundary in the suggestion text.
            // We also need to take into account, that could be a paragraph boundary.
            int nextWordBoundary = _currentSuggestion.Text.Span.IndexOf(' ');

            if (nextWordBoundary == -1)
            {
                nextWordBoundary = _currentSuggestion.LengthIncludingSymbol;
            }

            // Insert the suggestion up to the next word boundary
            int originalSelectionStart = SelectionStart;
            int originalSelectionLength = SelectionLength;

            SelectionStart = _currentSuggestion.StartIncludingSymbol;
            SelectionLength = nextWordBoundary;

            SelectedText = _currentSuggestion.Text.Span[..nextWordBoundary].ToString();

            // Restore the original selection
            SelectionStart = originalSelectionStart;
            SelectionLength = originalSelectionLength;

            // Update the current suggestion to reflect the accepted part
            _currentSuggestion = new TextSpan(
                _currentSuggestion.StartIncludingSymbol + nextWordBoundary,
                _currentSuggestion.LengthIncludingSymbol - nextWordBoundary,
                _currentSuggestion.Text[nextWordBoundary..]);
        }
    }

    private void ForceNewSuggestion()
    {
        RequestSuggestion();
    }

    private string OldParagraph => _oldParagraph;
    private bool IsCursorAtEnd => SelectionStart == Text.Length;

    private int CursorLocation
    {
        get
        {
            int paragraphStart = Text.LastIndexOf('\n', SelectionStart - 1) + 1;
            return SelectionStart - paragraphStart;
        }
    }
}
