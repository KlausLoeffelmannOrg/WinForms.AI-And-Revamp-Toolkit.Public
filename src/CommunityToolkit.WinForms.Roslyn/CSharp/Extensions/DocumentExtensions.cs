using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.WinForms.Roslyn.CSharp.Extensions
{
    /// <summary>
    ///  Extension methods for working with Roslyn Documents.
    /// </summary>
    public static class DocumentExtensions
    {
        // Private helper to extract all class declarations.
        private static async Task<IEnumerable<ClassDeclarationSyntax>> GetClassesAsync(
            this Document document)
        {
            var root = await document.GetSyntaxRootAsync().ConfigureAwait(false);

            return root?.DescendantNodes().OfType<ClassDeclarationSyntax>()
                ?? [];
        }

        /// <summary>
        ///  Gets the count of class declarations in the document.
        /// </summary>
        public static async Task<int> GetClassCountAsync(this Document document)
            => (await document.GetClassesAsync().ConfigureAwait(false)).Count();

        /// <summary>
        ///  Gets the first class declaration in the document.
        ///  Throws if none exist.
        /// </summary>
        public static async Task<ClassDeclarationSyntax> GetFirstClassAsync(
            this Document document)
        {
            var classes = await document.GetClassesAsync().ConfigureAwait(false);

            return classes.FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "The document does not contain any class declarations.");
        }

        /// <summary>
        ///  Gets the first class declaration in the document or null if none exist.
        /// </summary>
        public static async Task<ClassDeclarationSyntax?> GetFirstClassOrDefaultAsync(
            this Document document)
            => (await document.GetClassesAsync().ConfigureAwait(false)).FirstOrDefault();

        // Validates that the class is the only top-level class under its parent.
        private static ClassDeclarationSyntax ValidateSingleClass(
            ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration.Parent is not { } parent ||
                parent.ChildNodes().OfType<ClassDeclarationSyntax>().Count() != 1)
            {
                throw new InvalidOperationException(
                    "The document contains more than one class declaration.");
            }

            return classDeclaration;
        }

        /// <summary>
        ///  Gets the single top-level class declaration in the document.
        ///  Throws if none exist or if there is more than one.
        /// </summary>
        public static async Task<ClassDeclarationSyntax> GetSingleClassAsync(
            this Document document)
        {
            var classDeclaration = (await document.GetClassesAsync().ConfigureAwait(false))
                .FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "The document does not contain any class declarations.");

            return ValidateSingleClass(classDeclaration);
        }

        /// <summary>
        ///  Gets the single top-level class declaration in the document or null if none exist.
        ///  Throws if more than one class declaration is found.
        /// </summary>
        public static async Task<ClassDeclarationSyntax?> GetSingleClassOrDefaultAsync(
            this Document document)
        {
            var classDeclaration = (await document.GetClassesAsync().ConfigureAwait(false))
                .FirstOrDefault();

            return classDeclaration is null
                ? null
                : ValidateSingleClass(classDeclaration);
        }
    }
}
