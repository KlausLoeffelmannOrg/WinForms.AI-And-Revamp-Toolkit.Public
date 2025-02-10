using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.WinForms.Roslyn.CSharp.Extensions
{
    /// <summary>
    ///  Extension methods for working with Roslyn Documents.
    /// </summary>
    public static partial class DocumentExtensions
    {
        public static async Task<IEnumerable<TypeDeclarationSyntax>> GetTypesAndNestedTypesAsync(
            this Document document)
        {
            var root = await document.GetSyntaxRootAsync().ConfigureAwait(false);

            return root?.DescendantNodes().OfType<TypeDeclarationSyntax>()
                ?? [];
        }

        /// <summary>
        ///  Gets the count of type declarations in the document.
        /// </summary>
        public static async Task<int> GetTypeCountAsync(this Document document)
            => (await document.GetTypesAndNestedTypesAsync().ConfigureAwait(false)).Count();

        /// <summary>
        ///  Gets the first type declaration in the document.
        ///  Throws if none exist.
        /// </summary>
        public static async Task<TypeDeclarationSyntax> GetFirstTypeAsync(
            this Document document)
        {
            var types = await document.GetTypesAndNestedTypesAsync().ConfigureAwait(false);

            return types.FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "The document does not contain any type declarations.");
        }

        /// <summary>
        ///  Gets the first type declaration in the document or null if none exist.
        /// </summary>
        public static async Task<TypeDeclarationSyntax?> GetFirstTypeOrDefaultAsync(
            this Document document)
            => (await document.GetTypesAndNestedTypesAsync().ConfigureAwait(false)).FirstOrDefault();

        // Validates that the type is the only top-level type under its parent.
        private static TypeDeclarationSyntax ValidateSingleTypeDeclaration(
            TypeDeclarationSyntax typeDeclaration)
        {
            if (typeDeclaration.Parent is not { } parent ||
                parent.ChildNodes().OfType<TypeDeclarationSyntax>().Count() != 1)
            {
                throw new InvalidOperationException(
                    "The document contains more than one type declaration.");
            }

            return typeDeclaration;
        }

        /// <summary>
        ///  Gets the single top-level type declaration in the document.
        ///  Throws if none exist or if there is more than one.
        /// </summary>
        public static async Task<TypeDeclarationSyntax> GetSingleTypeAsync(
            this Document document)
        {
            var typeDeclaration = (await document.GetTypesAndNestedTypesAsync().ConfigureAwait(false))
                .FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "The document does not contain any type declarations.");

            return ValidateSingleTypeDeclaration(typeDeclaration);
        }

        /// <summary>
        ///  Gets the single top-level type declaration in the document or null if none exist.
        ///  Throws if more than one type declaration is found.
        /// </summary>
        public static async Task<TypeDeclarationSyntax?> GetSingleTypeOrDefaultAsync(
            this Document document)
        {
            var typeDeclaration = (await document.GetTypesAndNestedTypesAsync().ConfigureAwait(false))
                .FirstOrDefault();

            return typeDeclaration is null
                ? null
                : ValidateSingleTypeDeclaration(typeDeclaration);
        }
    }
}
