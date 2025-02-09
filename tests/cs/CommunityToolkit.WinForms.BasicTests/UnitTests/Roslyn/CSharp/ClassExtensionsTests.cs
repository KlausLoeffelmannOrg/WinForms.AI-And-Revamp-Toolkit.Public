using CommunityToolkit.WinForms.BasicTests.TestSupport;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CommunityToolkit.WinForms.Roslyn.CSharp.Extensions;

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

        string targetDirectory=TestDataDiscovery.GetTestDataDirectoryPath(
            subDirectoryPath: SubDirectoryPath);

        var data = new TheoryData<string>();

        // Iterate through all the files in the directory and make sure
        // that they exist. If they do, add them to the test data.
        // If they don't, we fail fast.
        foreach (string file in files)
        {
            string filePath = Path.Combine(targetDirectory, file);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"We have been trying to find the file '{filePath}', but it does not exist.");
            }

            data.Add(filePath);
        }

        return data;
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

        // Get the semantic model and the diagnostics:
        SemanticModel? semanticModel = await document.GetSemanticModelAsync();
        Assert.NotNull(semanticModel);

        // Get the class symbol
        INamedTypeSymbol? classSymbol = (INamedTypeSymbol?) semanticModel.GetDeclaredSymbol(classDeclaration);
        Assert.NotNull(classSymbol);
    }
}
