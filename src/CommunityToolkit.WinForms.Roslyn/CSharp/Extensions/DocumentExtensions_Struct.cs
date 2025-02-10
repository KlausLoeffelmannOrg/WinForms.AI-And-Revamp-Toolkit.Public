using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.WinForms.Roslyn.CSharp.Extensions
{
    /// <summary>
    ///  Extension methods for working with Roslyn Documents.
    /// </summary>
    public static partial class DocumentExtensions
    {
        public static async Task<IEnumerable<StructDeclarationSyntax>> GetStructsAndNestedStructsAsync(
            this Document document)
        {
            var root = await document.GetSyntaxRootAsync().ConfigureAwait(false);

            return root?.DescendantNodes().OfType<StructDeclarationSyntax>()
                ?? [];
        }

        /// <summary>
        ///  Gets the count of struct declarations in the document.
        /// </summary>
        public static async Task<int> GetStructCountAsync(this Document document)
            => (await document.GetStructsAndNestedStructsAsync().ConfigureAwait(false)).Count();

        /// <summary>
        ///  Gets the first struct declaration in the document.
        ///  Throws if none exist.
        /// </summary>
        public static async Task<StructDeclarationSyntax> GetFirstStructAsync(
            this Document document)
        {
            var structs = await document.GetStructsAndNestedStructsAsync().ConfigureAwait(false);

            return structs.FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "The document does not contain any struct declarations.");
        }

        /// <summary>
        ///  Gets the first struct declaration in the document or null if none exist.
        /// </summary>
        public static async Task<StructDeclarationSyntax?> GetFirstStructOrDefaultAsync(
            this Document document)
            => (await document.GetStructsAndNestedStructsAsync().ConfigureAwait(false)).FirstOrDefault();

        /// <summary>
        ///  Gets the single top-level struct declaration in the document.
        ///  Throws if none exist or if there is more than one.
        /// </summary>
        public static async Task<StructDeclarationSyntax> GetSingleStructAsync(
            this Document document)
        {
            var structDeclaration = (await document.GetStructsAndNestedStructsAsync().ConfigureAwait(false))
                .FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "The document does not contain any struct declarations.");

            return (StructDeclarationSyntax)ValidateSingleTypeDeclaration(structDeclaration);
        }

        /// <summary>
        ///  Gets the single top-level struct declaration in the document or null if none exist.
        ///  Throws if more than one struct declaration is found.
        /// </summary>
        public static async Task<StructDeclarationSyntax?> GetSingleStructOrDefaultAsync(
            this Document document)
        {
            var structDeclaration = (await document.GetStructsAndNestedStructsAsync().ConfigureAwait(false))
                .FirstOrDefault();

            return structDeclaration is null
                ? null
                : (StructDeclarationSyntax)ValidateSingleTypeDeclaration(structDeclaration);
        }
    }
}
