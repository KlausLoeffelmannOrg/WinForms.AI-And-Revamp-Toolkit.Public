using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.WinForms.Roslyn.CSharp.Extensions;

/// <summary>
///  Provides extension methods for <see cref="TypeDeclarationSyntax"/>.
/// </summary>
public static class TypeDeclarationExtensions
{
    public static HashSet<SyntaxKind> GetMemberKinds(
        this TypeDeclarationSyntax classDeclaration)
    {
        ArgumentNullException.ThrowIfNull(classDeclaration);

        HashSet<SyntaxKind> memberKinds = [];

        foreach (MemberDeclarationSyntax member in classDeclaration.Members)
        {
            memberKinds.Add(member.Kind());
        }

        return memberKinds;
    }

    public static IEnumerable<T> GetTypedMembersByKind<T>(
        this TypeDeclarationSyntax classDeclaration,
        SyntaxKind kind)
        where T : MemberDeclarationSyntax
    {
        ArgumentNullException.ThrowIfNull(classDeclaration);

        List<T> members = [.. classDeclaration.Members.Where(m => m.Kind() == kind).OfType<T>()];

        return members;
    }

    /// <summary>
    ///  Gets all members of a type grouped by their <see cref="SyntaxKind"/>.
    ///  Each inner dictionary is keyed by a member “name” (or a fallback key)
    /// to ensure uniqueness.
    /// </summary>
    /// <param name="typeDeclaration">The class or struct declaration.</param>
    /// <returns>
    ///  A dictionary mapping <see cref="SyntaxKind"/> to a dictionary that maps a string key
    ///  to the <see cref="MemberDeclarationSyntax"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///  Thrown if <paramref name="typeDeclaration"/> is null.
    /// </exception>
    public static Dictionary<SyntaxKind, Dictionary<string, MemberDeclarationSyntax>>
        GetMemberDictionary(this TypeDeclarationSyntax typeDeclaration)
    {
        ArgumentNullException.ThrowIfNull(typeDeclaration);

        Dictionary<SyntaxKind, Dictionary<string, MemberDeclarationSyntax>> result = [];

        foreach (MemberDeclarationSyntax member in typeDeclaration.Members)
        {
            SyntaxKind kind = member.Kind();

            if (!result.TryGetValue(kind, out Dictionary<string, MemberDeclarationSyntax>? innerDict))
            {
                innerDict = [];
                result[kind] = innerDict;
            }

            string key = GetMemberKey(member);

            // Ensure key uniqueness if needed.
            if (innerDict.ContainsKey(key))
            {
                int count = 1;
                string newKey;

                do
                {
                    newKey = key + "_" + count;
                    count++;
                } while (innerDict.ContainsKey(newKey));

                key = newKey;
            }

            innerDict[key] = member;
        }

        return result;
    }

    /// <summary>
    ///  Flattens a dictionary of grouped type members into a single enumerable.
    /// </summary>
    /// <param name="memberDict">
    ///  The dictionary grouping members by <see cref="SyntaxKind"/>.
    /// </param>
    /// <returns>
    ///  An enumerable of all <see cref="MemberDeclarationSyntax"/> found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///  Thrown if <paramref name="memberDict"/> is null.
    /// </exception>
    public static IEnumerable<MemberDeclarationSyntax> FlattenKindsToMembers(
        this Dictionary<SyntaxKind, Dictionary<string, MemberDeclarationSyntax>> memberDict)
    {
        return memberDict is null
            ? throw new ArgumentNullException(nameof(memberDict))
            : memberDict.Values.SelectMany(inner => inner.Values);
    }

    /// <summary>
    ///  Attempts to retrieve a “name” for the member.
    ///  For known types (methods, properties, etc.) the identifier is used.
    ///  Otherwise, a fallback key is created.
    /// </summary>
    /// <param name="member">The member declaration.</param>
    /// <returns>
    ///  A string key representing the member.
    /// </returns>
    private static string GetMemberKey(MemberDeclarationSyntax member)
    {
        switch (member)
        {
            case MethodDeclarationSyntax method:
                return method.Identifier.ValueText;
            case ConstructorDeclarationSyntax ctor:
                return ctor.Identifier.ValueText;
            case DestructorDeclarationSyntax destructor:
                return destructor.Identifier.ValueText;
            case PropertyDeclarationSyntax prop:
                return prop.Identifier.ValueText;
            case EventDeclarationSyntax evt:
                return evt.Identifier.ValueText;
            case FieldDeclarationSyntax field:
                // For fields, combine all variable names.
                IEnumerable<string>? names = field.Declaration?.Variables
                    .Select(v => v.Identifier.ValueText)
                    .Where(n => !string.IsNullOrWhiteSpace(n));
                if (names is not null && names.Any())
                    return string.Join(", ", names);
                break;
            case DelegateDeclarationSyntax del:
                return del.Identifier.ValueText;
            case EnumMemberDeclarationSyntax enumMember:
                return enumMember.Identifier.ValueText;
        }

        // Fallback: use the kind name and the starting position.
        return $"{member.Kind()}_{member.SpanStart}";
    }
}
