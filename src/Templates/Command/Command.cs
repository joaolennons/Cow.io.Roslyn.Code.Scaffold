using System;
using Coding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Templates
{
    public class Command : ISourceCode
    {
        private readonly string _namespace;
        private readonly string _name;

        public string FileName => $"{_name}.cs";
        public string Folder => Folders.Commands;

        public Command(string @namespace, string name, Action action)
        {
            _namespace = @namespace;
            _name = $"{action}{name}{nameof(Command)}";
        }

        public string Code
        {
            get
            {
                return Namespace
                    .AddUsings(System, MediatR)
                    .AddMembers(
                        Class.Inherits(TemplateConsts.IRequest(nameof(Guid))))
                    .NormalizeWhitespace()
                    .ToFullString();
            }
        }

        private NamespaceDeclarationSyntax Namespace => SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_namespace)).NormalizeWhitespace();
        private UsingDirectiveSyntax System => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(nameof(System)));
        private UsingDirectiveSyntax MediatR => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(nameof(MediatR)));
        private ClassDeclarationSyntax Class => SyntaxFactory.ClassDeclaration(_name).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
    }
}

