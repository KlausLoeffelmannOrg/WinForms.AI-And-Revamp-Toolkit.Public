using Microsoft.SemanticKernel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CommunityToolkit.WinForms.AI;

internal static class ReturnTokenParser
{
    /// <summary>
    ///  Processes tokens from an async enumerable of StreamingChatMessageContent.
    /// </summary>
    /// <param name="asyncEnumerable">The async enumerable of StreamingChatMessageContent to process.</param>
    /// <param name="onReceivedMetaDataAction">Action to invoke when metadata is received.</param>
    /// <param name="onReceivedNextParagraphAction">Action to invoke when a new paragraph is received.</param>
    /// <returns>An async enumerable of processed tokens as strings.</returns>
    /// <remarks>
    ///  <para>
    ///   This method processes tokens from an async enumerable of StreamingChatMessageContent. It iterates through the characters of the input strings,
    ///   building words and metadata blocks. When a word boundary is detected, the word is yielded. Metadata blocks are identified by
    ///   curly braces and are processed separately.
    ///  </para>
    ///   <para>
    ///   The method also handles paragraph boundaries, yielding paragraphs when a newline character is encountered.
    ///   Metadata is raised through the <see cref="AsyncReceivedMetaDataEventArgs"/> event, and paragraphs are raised through
    ///   the <see cref="AsyncReceivedNextParagraphEventArgs"/> event.
    ///  </para>
    /// </remarks>
    public static async IAsyncEnumerable<string> ProcessTokens(
        IAsyncEnumerable<StreamingChatMessageContent> asyncEnumerable,
        Action<AsyncReceivedMetaDataEventArgs> onReceivedMetaDataAction,
        Action<AsyncReceivedNextParagraphEventArgs> onReceivedNextParagraphAction)
    {
        var wordBuilder = new StringBuilder();
        var metaDataBuilder = new StringBuilder();
        var paragraphBuilder = new StringBuilder();
        int positionCounter = 0;
        bool inMeta = false;
        bool pendingOpen = false;   // waiting for second '{'
        bool pendingClose = false;  // waiting for second '}'

        await foreach (char c in ConvertToCharStreamAsync(asyncEnumerable))
        {
            if (!inMeta)
            {
                // Handle potential metadata opening.
                if (pendingOpen)
                {
                    if (c == '{')
                    {
                        // Start metadata block.
                        inMeta = true;
                        pendingOpen = false;
                        metaDataBuilder.Clear();
                        continue;
                    }
                    else
                    {
                        // False alarm: output the pending '{' then process current char.
                        wordBuilder.Append('{');
                        positionCounter++;
                        pendingOpen = false;

                        // Fall through to process current char normally.
                    }
                }

                if (c == '{')
                {
                    pendingOpen = true;
                    continue;
                }
            }
            else
            {
                // We are inside a metadata block.
                if (pendingClose)
                {
                    if (c == '}')
                    {
                        // End metadata block.
                        inMeta = false;
                        pendingClose = false;

                        onReceivedMetaDataAction(new AsyncReceivedMetaDataEventArgs(
                            metaDataBuilder.ToString(),
                            positionCounter));

                        metaDataBuilder.Clear();
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

            // Normal text processing.
            positionCounter++;
            wordBuilder.Append(c);

            if (IsWordSeparator(c) && wordBuilder.Length > 0)
            {
                yield return ProcessWord(wordBuilder.ToString(), c);
                wordBuilder.Clear();
            }
        }

        // Handle any pending open brace.
        if (pendingOpen)
        {
            wordBuilder.Append('{');
        }

        // Flush any remaining word.
        if (wordBuilder.Length > 0)
        {
            yield return ProcessWord(wordBuilder.ToString(), null);
        }

        // Local function to process a completed word.
        string ProcessWord(string word, char? lastChar)
        {
            paragraphBuilder.Append(word);
            var paragraph = paragraphBuilder.ToString();

            if (lastChar is null or '\r' or '\n')
            {
                onReceivedNextParagraphAction(new AsyncReceivedNextParagraphEventArgs(
                    paragraph: paragraph,
                    textPosition: positionCounter,
                    isLastParagraph: lastChar is null));

                paragraphBuilder.Clear();
            }

            return word;
        }
    }

    /// <summary>
    ///  Determines whether a character is considered a word separator.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>True if the character is a word separator; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool IsWordSeparator(char c) => c switch
    {
        ' ' or '\n' or '\r' => true,
        _ => false,
    };

    /// <summary>
    ///  Converts an async enumerable of StreamingChatMessageContent to an async enumerable of characters.
    /// </summary>
    /// <param name="asyncEnumerable">The async enumerable of StreamingChatMessageContent to convert.</param>
    /// <returns>An async enumerable of characters.</returns>
    static async IAsyncEnumerable<char> ConvertToCharStreamAsync(
        IAsyncEnumerable<StreamingChatMessageContent> asyncEnumerable)
    {
        await foreach (var response in asyncEnumerable)
        {
            if (response?.Content is not null)
            {
                foreach (char c in response.Content)
                {
                    yield return c;
                }
            }
        }
    }
}
