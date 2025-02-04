using System.Text;

/// <summary>
/// Provides conversion between actual strings and their C# literal representations.
/// </summary>
public static class StringLiteralConverterExtension
{
    /// <summary>
    /// Converts a C# string literal (with escape sequences) into the actual string.
    /// The literal must be enclosed in double quotes.
    /// </summary>
    /// <param name="literal">The C# string literal to convert.</param>
    /// <returns>The actual string represented by the literal.</returns>
    /// <exception cref="ArgumentNullException">Thrown if literal is null.</exception>
    /// <exception cref="ArgumentException">
    /// Thrown if literal is not a valid C# string literal.
    /// </exception>
    public static string? FromCSharpLiteral(this string? literal)
    {
        if (literal == "null")
        {
            return null;
        }

        ArgumentNullException.ThrowIfNull(literal);

        if (literal.Length < 2 || literal[0] != '"' || literal[^1] != '"')
        {
            throw new ArgumentException(
                message: "Input is not a valid C# string literal.",
                paramName: nameof(literal));
        }

        string inner = literal[1..^1];
        StringBuilder sb = new();

        for (int i = 0; i < inner.Length; i++)
        {
            char ch = inner[i];
            if (ch != '\\')
            {
                sb.Append(ch);
            }
            else
            {
                // Advance to the escape character.
                i++;

                if (i >= inner.Length)
                {
                    throw new ArgumentException(
                        message: "Invalid escape sequence at end of literal.",
                        paramName: nameof(literal));
                }

                char next = inner[i];

                sb.Append(next switch
                {
                    '\\' => '\\',
                    '\"' => '\"',
                    '`' => '`',
                    '0' => '\0',
                    'a' => '\a',
                    'b' => '\b',
                    'f' => '\f',
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    'v' => '\v',
                    'u' => ParseUnicodeEscape(inner, ref i),
                    _ => throw new ArgumentException(
                        message: $"Unknown escape sequence: \\{next}",
                        paramName: nameof(literal))
                });
            }
        }

        return sb.ToString();

        // Local function to parse a Unicode escape sequence.
        static char ParseUnicodeEscape(string s, ref int index)
        {
            const int hexDigits = 4;

            if (index + hexDigits >= s.Length)
            {
                throw new ArgumentException(
                    message: "Incomplete Unicode escape sequence.",
                    paramName: nameof(literal));
            }
            string hex = s.Substring(index + 1, hexDigits);
            if (!int.TryParse(
                s: hex,
                style: System.Globalization.NumberStyles.HexNumber,
                provider: null,
                result: out int code))
            {
                throw new ArgumentException(
                    message: "Invalid Unicode escape sequence.",
                    paramName: nameof(literal));
            }

            index += hexDigits;
            return (char)code;
        }
    }

    /// <summary>
    /// Converts an actual string into its C# string literal representation.
    /// Characters with code &lt; 32 or &gt; 255 are escaped, along with " and \.
    /// </summary>
    /// <param name="input">The input string to convert.</param>
    /// <returns>The C# string literal representing the input.</returns>
    /// <exception cref="ArgumentNullException">Thrown if input is null.</exception>
    public static string ToCSharpLiteral(this string? input)
    {
        if (input is null)
        {
            return "null";
        }

        StringBuilder sb = new();
        sb.Append('"');

        foreach (char ch in input)
        {
            sb.Append(ch switch
            {
                '\"' => "\\u0022",
                '\\' => "\\\\",
                '`' => "\\u0060",
                _ when (ch < ' ' || ch > (char)255) => ch switch
                {
                    '\0' => "\\0",
                    '\a' => "\\a",
                    '\b' => "\\b",
                    '\f' => "\\f",
                    '\n' => "\\n",
                    '\r' => "\\r",
                    '\t' => "\\t",
                    '\v' => "\\v",
                    _ => $"\\u{(int)ch:X4}"
                },
                _ => ch.ToString()
            });
        }

        sb.Append('"');
        return sb.ToString();
    }
}
