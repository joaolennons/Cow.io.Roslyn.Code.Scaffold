using Coding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Templates
{
    public class QueryHandler : ISourceCode
    {
        private readonly string _namespace;
        private readonly string _name;
        private readonly string _queryRequest;
        private readonly string _queryResponse;

        public string FileName => $"{_name}.cs";
        public string Folder => Folders.Queries;

        public QueryHandler(string @namespace, string name)
        {
            _namespace = @namespace;
            _name = $"{name}QueryHandler";
            _queryRequest = $"{name}Query";
            _queryResponse = $"{name}Response";
        }

        public string Code
        {
            get
            {
                return Namespace
                    .AddUsings(MediatR, Tasks, Threading)
                    .AddMembers(
                        Class.Inherits(TemplateConsts.IRequestHandler(_queryRequest, _queryResponse))
                        .AddMembers(Class.WithAsyncMethod("Handle", _queryResponse, $"request:{_queryRequest}")))
                    .NormalizeWhitespace()
                    .ToFullString();
            }
        }

        private NamespaceDeclarationSyntax Namespace => SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_namespace)).NormalizeWhitespace();
        private UsingDirectiveSyntax MediatR => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(nameof(MediatR)));
        private UsingDirectiveSyntax Tasks => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading.Tasks"));
        private UsingDirectiveSyntax Threading => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading"));
        private ClassDeclarationSyntax Class => SyntaxFactory.ClassDeclaration(_name).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
    }
}
