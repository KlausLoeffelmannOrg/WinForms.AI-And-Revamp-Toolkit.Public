using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.WinForms.Roslyn.CSharp.Extensions;

public static class ClassExtensions
{
    public static HashSet<SyntaxKind> GetMemberKinds(this ClassDeclarationSyntax classDeclaration)
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
        this ClassDeclarationSyntax classDeclaration, 
        SyntaxKind kind) 
        where T : MemberDeclarationSyntax
    {
        ArgumentNullException.ThrowIfNull(classDeclaration);

        var members = new List<T>();

        members.AddRange(classDeclaration.Members.Where(m => m.Kind() == kind).OfType<T>());

        return members;
    }
}
