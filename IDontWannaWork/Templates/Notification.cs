using Coding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Templates
{
    public class Notification : ISourceCode
    {
        private readonly string _namespace;
        private readonly string _name;

        public string FileName => $"{_name}.cs";
        public string Folder => Folders.Notifications;

        public Notification(string @namespace, string name)
        {
            _namespace = @namespace;
            _name = $"{name}{nameof(Notification)}";
        }

        public string Code
        {
            get
            {
                return Namespace
                    .AddUsings(System, MediatR)
                    .AddMembers(
                        QueryClass.Inherits(TemplateConsts.INotification))
                    .NormalizeWhitespace()
                    .ToFullString();
            }
        }

        private NamespaceDeclarationSyntax Namespace => SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_namespace)).NormalizeWhitespace();
        private UsingDirectiveSyntax System => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(nameof(System)));
        private UsingDirectiveSyntax MediatR => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(nameof(MediatR)));
        private ClassDeclarationSyntax QueryClass => SyntaxFactory.ClassDeclaration(_name).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
    }
}
