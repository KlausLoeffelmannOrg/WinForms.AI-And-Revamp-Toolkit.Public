using CommunityToolkit.WinForms.BasicTests.TestSupport;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CommunityToolkit.WinForms.Roslyn.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp;

namespace CommunityToolkit.WinForms.BasicTests.UnitTests.Roslyn.CSharp;

public class ClassExtensionsTests
{
    private const string SubDirectoryPath = "Roslyn/CSharp";

    public static TheoryData<string> GetTestFiles()
    {
        List<string> files =
        [
            "Class_with_every_member_type_relevant.cs"
        ];

        string targetDirectory = TestDataDiscovery.GetTestDataDirectoryPath(
            subDirectoryPath: SubDirectoryPath);

        return TestDataDiscovery.GetTheoryDataFromFiles(files, targetDirectory);
    }

    [Theory]
    [MemberData(nameof(GetTestFiles))]
    public async Task CS_ClassExtensions_ListOfMemberKinds(string filename)
    {
        AdhocWorkspace workspace = null!;
        workspace = workspace.CreateWorkspaceFromSourceFiles([filename]);

        Document document = workspace.GetFirstDocument();
        Assert.NotNull(document);

        // Get the class declaration
        ClassDeclarationSyntax classDeclaration = await document.GetSingleClassAsync();

        // Get the member kinds of the class:
        HashSet<SyntaxKind> memberKinds = classDeclaration.GetMemberKinds();

        // Arrange
        var expectedMemberKinds = new HashSet<SyntaxKind>
        {
            SyntaxKind.FieldDeclaration,
            SyntaxKind.ConstructorDeclaration,
            SyntaxKind.MethodDeclaration,
            SyntaxKind.ClassDeclaration
        };

        // Assert
        Assert.NotNull(memberKinds);

        Assert.Equal(
            expected: expectedMemberKinds.Count,
            actual: memberKinds.Count);

        Assert.True(
            condition: expectedMemberKinds.SetEquals(memberKinds),
            userMessage: "The member kinds do not match the expected kinds.");

        // Get the semantic model and the diagnostics:
        SemanticModel? semanticModel = await document.GetSemanticModelAsync();
        Assert.NotNull(semanticModel);

        // Get the class symbol
        INamedTypeSymbol? classSymbol = (INamedTypeSymbol?)semanticModel.GetDeclaredSymbol(classDeclaration);
        Assert.NotNull(classSymbol);
    }
}
