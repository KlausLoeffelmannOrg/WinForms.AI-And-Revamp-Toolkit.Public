using Microsoft.SemanticKernel;
using System.Text;

namespace CommunityToolkit.WinForms.AI.ConverterLogic;

internal static class ReturnTokenParser
{
#if COLLECT_TEST_DATA
    private static readonly List<string> s_testDataCollector = [];
#endif

    /// <summary>
    /// Processes tokens from an async enumerable of StreamingChatMessageContent.
    /// </summary>
    /// <param name="asyncEnumerable">The async enumerable of StreamingChatMessageContent to process.</param>
    /// <param name="onReceivedMetaDataAction">Action to invoke when metadata is received.</param>
    /// <param name="onReceivedNextParagraphAction">Action to invoke when a new paragraph is received.</param>
    /// <returns>An async enumerable of processed tokens as strings.</returns>
    public static async IAsyncEnumerable<string> ProcessTokens(
        IAsyncEnumerable<StreamingChatMessageContent> asyncEnumerable,
        Action<AsyncReceivedInlineMetaDataEventArgs> onReceivedMetaDataAction,
        Action<AsyncReceivedNextParagraphEventArgs> onReceivedNextParagraphAction,
        Action<AsyncCodeBlockInfoProvidedEventArgs> onCodeBlockInfoProvidedAction)
    {
        StringBuilder wordBuilder = new();
        StringBuilder paragraphBuilder = new();
        StringBuilder metaDataBuilder = new();
        StringBuilder? codeBlockBuilder = null;

        int positionCounter = 0;

        // State flags.
        bool inMeta = false;
        bool pendingOpen = false;   // waiting for second '{'
        bool pendingClose = false;  // waiting for second '}'
        bool metaCandidateDedicated = false;
        bool isInlineCode = false;        // new flag to track code blocks/spans

        CodeBlockInfo? currentCodeBlock = null;
        int nestedCodeBlockCounter = 0;

        // Used to “peek” ahead.
        char? pendingChar = null;

#if COLLECT_TEST_DATA
        s_testDataCollector.Clear();
        await foreach (var token in asyncEnumerable)
        {
            s_testDataCollector.Add(token.Content.ToCSharpLiteral());
        }
#endif

        // Get the char stream.
        await using IAsyncEnumerator<char> enumerator =
            ConvertToCharStreamAsync(asyncEnumerable).GetAsyncEnumerator();

        while (true)
        {
            char c;

            if (pendingChar is not null)
            {
                c = pendingChar.Value;
                pendingChar = null;
            }
            else if (await enumerator.MoveNextAsync())
            {
                c = enumerator.Current;
            }
            else
            {
                break;
            }

            // --- Code detection (simple inline code using backticks) ---
            if (c == '`' && nestedCodeBlockCounter == 0)
            {
                isInlineCode = !isInlineCode;
                // Always output the backtick.
                positionCounter++;
                wordBuilder.Append(c);
                paragraphBuilder.Append(c);

                continue;
            }

            // --- Only process potential meta markers if not in code ---
            if (!inMeta && !isInlineCode)
            {
                if (pendingOpen)
                {
                    if (c == '{')
                    {
                        // Start metadata block.
                        inMeta = true;
                        pendingOpen = false;
                        metaDataBuilder.Clear();

                        // If no text has been appended to the paragraph, mark as dedicated.
                        metaCandidateDedicated = paragraphBuilder.Length == 0;

                        continue;
                    }
                    else
                    {
                        // False alarm: output the pending '{' and process current char.
                        wordBuilder.Append('{');
                        positionCounter++;
                        pendingOpen = false;
                        // Fall through to normal processing.
                    }
                }

                if (c == '{')
                {
                    pendingOpen = true;
                    continue;
                }
            }
            else if (inMeta)
            {
                // We're inside a metadata token stream: {{key:value}}.
                if (pendingClose)
                {
                    if (c == '}')
                    {
                        // End metadata block.
                        inMeta = false;
                        pendingClose = false;

                        string metaContent = metaDataBuilder.ToString();
                        // Parse key and value.
                        int colonIndex = metaContent.IndexOf(':');

                        string key = colonIndex >= 0
                            ? metaContent[..colonIndex].Trim()
                            : metaContent.Trim();

                        string value = colonIndex >= 0
                            ? metaContent[(colonIndex + 1)..].Trim()
                            : string.Empty;

                        bool wasDedicated = false;

                        (pendingChar, wasDedicated) = await ProcessMetadataTokenStream(
                            metaCandidateDedicated,
                            pendingChar,
                            enumerator,
                            wasDedicated);

                        ReturnStreamMetaData metaDataItem =
                            new(key, value, positionCounter, wasDedicated);

                        onReceivedMetaDataAction(
                            new AsyncReceivedInlineMetaDataEventArgs(metaDataItem));

                        metaDataBuilder.Clear();
                        metaCandidateDedicated = false;

                        continue;
                    }
                    else
                    {
                        // False alarm: include the pending '}'.
                        metaDataBuilder.Append('}');
                        pendingClose = false;
                        // Process current char as metadata content.
                    }
                }

                if (c == '}')
                {
                    pendingClose = true;
                    continue;
                }

                metaDataBuilder.Append(c);

                continue;
            }

            // --- Normal text processing (including when inCode) ---
            positionCounter++;
            wordBuilder.Append(c);
            paragraphBuilder.Append(c);

            if (IsWordSeparator(c) && wordBuilder.Length > 0)
            {
                yield return ProcessWordParagraphCodeBlock(wordBuilder.ToString(), c);

                wordBuilder.Clear();
            }
        }

        // Handle any pending open brace.
        if (pendingOpen)
        {
            wordBuilder.Append('{');
        }

        if (wordBuilder.Length > 0)
        {
            yield return ProcessWordParagraphCodeBlock(wordBuilder.ToString(), null);
        }

        // Local function to process a completed word.
        string ProcessWordParagraphCodeBlock(string word, char? lastChar)
        {
            if (lastChar is null or '\r' or '\n')
            {
                string paragraph = paragraphBuilder.ToString();

                if (IsCodeBlockInitiator(paragraph, out CodeBlockInfo? codeBlockInfo))
                {
                    if (nestedCodeBlockCounter == 0)
                    {
                        currentCodeBlock = codeBlockInfo;
                        codeBlockBuilder = new();
                    }

                    nestedCodeBlockCounter++;
                    isInlineCode = true;
                }
                else
                {
                    string trimmedParagraph = paragraph.Trim();
                    if (trimmedParagraph.StartsWith("```"))
                    {
                        if (nestedCodeBlockCounter == 1)
                        {
                            isInlineCode = false;

                            CodeBlockInfo codeBlock = currentCodeBlock
                                ?? throw new InvalidOperationException("Code block not initialized.");

                            codeBlock.Code = codeBlockBuilder?.ToString()
                                ?? throw new InvalidOperationException("Code block content not initialized.");

                            onCodeBlockInfoProvidedAction(
                                new AsyncCodeBlockInfoProvidedEventArgs(
                                    codeBlock));
                        }
                        else
                        {
                            codeBlockBuilder?.Append(paragraph);
                        }

                        nestedCodeBlockCounter--;
                    }
                }

                onReceivedNextParagraphAction(new AsyncReceivedNextParagraphEventArgs(
                        paragraph: paragraphBuilder.ToString(),
                        textPosition: positionCounter,
                        isLastParagraph: lastChar is null));

                paragraphBuilder.Clear();
            }

            return word;
        }

        static async Task<(char? pendingChar, bool wasDedicated)> ProcessMetadataTokenStream(
            bool metaCandidateDedicated,
            char? pendingChar,
            IAsyncEnumerator<char> enumerator,
            bool wasDedicated)
        {
            // Peek ahead to see if the metadata token is alone on the line.
            if (metaCandidateDedicated && await enumerator.MoveNextAsync())
            {
                char next = enumerator.Current;

                if (next is not '\r' and not '\n')
                {
                    pendingChar = next;
                }
                else
                {
                    wasDedicated = true;

                    if (next == '\r' && await enumerator.MoveNextAsync())
                    {
                        char after = enumerator.Current;

                        if (after != '\n')
                        {
                            pendingChar = after;
                        }
                    }
                }
            }

            return (pendingChar, wasDedicated);
        }
    }

    private static bool IsCodeBlockInitiator(string paragraph, out CodeBlockInfo? codeBlock)
    {
        codeBlock = null;

        // Let's use a Span<char> to avoid allocations:
        // A code-block can be initiated with 3 or 4 ticks, followed by a code-block descriptor
        // (e.g., "csharp" or "cs" for C# code) and a newline.
        ReadOnlySpan<char> span = paragraph.AsSpan();

        if (span.Length < 5)
        {
            return false;
        }

        // We need to _ensure_ 3 or 4 ticks and then _get_ the descriptor.
        // However, a 5th tick is not allowed - then this would not be a code block start.
        if (span[0] != '`' || span[1] != '`' || span[2] != '`')
        {
            return false;
        }

        int tickCount = 3;
        if (span.Length > 3 && span[3] == '`')
        {
            tickCount++;
        }

        // Ensure no 5th tick
        if (span.Length > tickCount && span[tickCount] == '`')
        {
            return false;
        }

        // Check for a valid descriptor after the ticks
        ReadOnlySpan<char> descriptor = span[tickCount..].Trim();

        if (descriptor.Length == 0 || descriptor.Contains(' '))
        {
            return false;
        }

        codeBlock = new CodeBlockInfo
        {
            Code = string.Empty,
            Language = descriptor.ToString(),
            Title = string.Empty,
            Filename = string.Empty
        };

        return true;
    }

    /// <summary>
    /// Determines if the given character is a word separator.
    /// </summary>
    private static bool IsWordSeparator(char c) =>
        char.IsWhiteSpace(c);

    /// <summary>
    /// Converts an async enumerable of StreamingChatMessageContent to an async enumerable of characters.
    /// </summary>
    /// <param name="asyncEnumerable">The async enumerable of StreamingChatMessageContent to convert.</param>
    /// <returns>An async enumerable of characters.</returns>
    static async IAsyncEnumerable<char> ConvertToCharStreamAsync(
        IAsyncEnumerable<StreamingChatMessageContent> asyncEnumerable)
    {
        await foreach (StreamingChatMessageContent response in asyncEnumerable)
        {
            if (response?.Content is null)
            {
                continue;
            }

            foreach (char c in response.Content)
            {
                yield return c;
            }
        }
    }
}
