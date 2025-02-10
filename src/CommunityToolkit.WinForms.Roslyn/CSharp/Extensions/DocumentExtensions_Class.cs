using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.WinForms.Roslyn.CSharp.Extensions
{
    /// <summary>
    ///  Extension methods for working with Roslyn Documents.
    /// </summary>
    public static partial class DocumentExtensions
    {
        public static async Task<IEnumerable<ClassDeclarationSyntax>> GetClassesAsync(
            this Document document)
        {
            // Find the "level" with the first class:
            var root = await document.GetSyntaxRootAsync().ConfigureAwait(false);

            var first = root?.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();

            if (first is not null)
            {
                // Get the parent and then all classes in that level:
                var parent = first.Parent
                    ?? throw new NullReferenceException(nameof(first));

                return parent.ChildNodes().OfType<ClassDeclarationSyntax>();
            }

            return [];
        }

        public static async Task<IEnumerable<ClassDeclarationSyntax>> GetClassesAndNestedClassesAsync(
            this Document document)
        {
            var root = await document
                .GetSyntaxRootAsync()
                .ConfigureAwait(false);

            return root?.DescendantNodes().OfType<ClassDeclarationSyntax>()
                ?? [];
        }

        /// <summary>
        ///  Gets the count of class declarations in the document.
        /// </summary>
        public static async Task<int> GetClassCountAsync(this Document document)
            => (await document.GetClassesAndNestedClassesAsync().ConfigureAwait(false)).Count();

        /// <summary>
        ///  Gets the first class declaration in the document.
        ///  Throws if none exist.
        /// </summary>
        public static async Task<ClassDeclarationSyntax> GetFirstClassAsync(
            this Document document)
        {
            var classes = await document.GetClassesAndNestedClassesAsync().ConfigureAwait(false);

            return classes.FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "The document does not contain any class declarations.");
        }

        /// <summary>
        ///  Gets the first class declaration in the document or null if none exist.
        /// </summary>
        public static async Task<ClassDeclarationSyntax?> GetFirstClassOrDefaultAsync(
            this Document document)
            => (await document.GetClassesAndNestedClassesAsync().ConfigureAwait(false)).FirstOrDefault();

        /// <summary>
        ///  Gets the single top-level class declaration in the document.
        ///  Throws if none exist or if there is more than one.
        /// </summary>
        public static async Task<ClassDeclarationSyntax> GetSingleClassAsync(
            this Document document)
        {
            var classDeclaration = (await document.GetClassesAndNestedClassesAsync().ConfigureAwait(false))
                .FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "The document does not contain any class declarations.");

            return (ClassDeclarationSyntax)ValidateSingleTypeDeclaration(classDeclaration);
        }

        /// <summary>
        ///  Gets the single top-level class declaration in the document or null if none exist.
        ///  Throws if more than one class declaration is found.
        /// </summary>
        public static async Task<ClassDeclarationSyntax?> GetSingleClassOrDefaultAsync(
            this Document document)
        {
            var classDeclaration = (await document.GetClassesAndNestedClassesAsync().ConfigureAwait(false))
                .FirstOrDefault();

            return classDeclaration is null
                ? null
                : (ClassDeclarationSyntax)ValidateSingleTypeDeclaration(classDeclaration);
        }
    }
}
