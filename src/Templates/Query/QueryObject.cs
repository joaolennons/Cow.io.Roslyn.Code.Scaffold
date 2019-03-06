using Coding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Templates
{
    public class QueryObject : ISourceCode
    {
        private readonly string _namespace;
        private readonly string _queryName;
        private readonly string _queryResponse;

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
                return Namespace
                    .AddUsings(Usings)
                    .AddMembers(
                        QueryClass.Inherits(TemplateConsts.IRequest(_queryResponse)))
                    .AddMembers(ResponseClass)
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
