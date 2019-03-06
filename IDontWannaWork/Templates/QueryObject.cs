using Coding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Templates
{
    public class QueryObject : ISourceCode
    {
        private string _namespace;
        private string _queryName;
        private string _queryResponse;

        public string FileName => $"{_queryName}.cs";
        public string Folder => Folders.Queries;

        public QueryObject(string @namespace, string name)
        {
            _namespace = @namespace;
            _queryName = $"{name}Query";
            _queryResponse = $"{name}Response";
        }

        public string Code
        {
            get
            {
                // Create a Property: (public int Quantity { get; set; })
                var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName("int"), "Quantity")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddAccessorListAccessors(SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                return Namespace
                    .AddUsings(Usings)
                    .AddMembers(
                        QueryClass.Inherits(TemplateConsts.IRequest(_queryResponse))
                        .AddMembers(propertyDeclaration))
                    .AddMembers(
                        ResponseClass
                        .AddMembers(propertyDeclaration))
                    .NormalizeWhitespace()
                    .ToFullString();
            }
        }

        private NamespaceDeclarationSyntax Namespace => SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_namespace)).NormalizeWhitespace();
        private UsingDirectiveSyntax Usings => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
        private ClassDeclarationSyntax QueryClass => SyntaxFactory.ClassDeclaration(_queryName).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
        private ClassDeclarationSyntax ResponseClass => SyntaxFactory.ClassDeclaration(_queryResponse).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
    }
}
