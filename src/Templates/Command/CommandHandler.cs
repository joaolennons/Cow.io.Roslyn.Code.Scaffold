using Coding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace Templates
{
    public class CommandHandler : ISourceCode
    {
        private readonly string _namespace;
        private readonly string _name;
        private readonly Action _action;

        public string FileName => $"{_name}.cs";
        public string Folder => Folders.Commands;

        public CommandHandler(string @namespace, string name, Action action)
        {
            _action = action;
            _namespace = @namespace;
            _name = $"{name}{nameof(CommandHandler)}";
        }

        public string Code
        {
            get
            {
                var inheritances = new List<string>();
                if (_action.HasFlag(Action.Create))
                    inheritances.Add(TemplateConsts.IRequestHandler(FormatInheritance(Action.Create), nameof(Guid)));
                if (_action.HasFlag(Action.Update))
                    inheritances.Add(TemplateConsts.IRequestHandler(FormatInheritance(Action.Update), nameof(Guid)));
                if (_action.HasFlag(Action.Delete))
                    inheritances.Add(TemplateConsts.IRequestHandler(FormatInheritance(Action.Delete), nameof(Guid)));

                var @class = Class.Inherits(inheritances.ToArray());

                //private readonly IMediator _mediator
                var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName("IMediator"), "_mediator")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword), SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                var @params = new List<ParameterSyntax>
                {
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("mediator")).WithType(SyntaxFactory.ParseTypeName("IMediator"))
                };
                var @body = SyntaxFactory.Block(SyntaxFactory.ParseStatement("_mediator = mediator;"));

                @class = @class.AddMembers(propertyDeclaration,
                SyntaxFactory.ConstructorDeclaration(_name)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddParameterListParameters(@params.ToArray())
                    .WithBody(@body));

                if (_action.HasFlag(Action.Create))
                    @class = @class.AddMembers(Class.WithAsyncNotificationMethod<Guid>("Handle", FormatNotification(Action.Create), parameters: $"{nameof(Command).ToLower()}:{FormatInheritance(Action.Create)}"));

                if (_action.HasFlag(Action.Update))
                    @class = @class.AddMembers(Class.WithAsyncNotificationMethod<Guid>("Handle", FormatNotification(Action.Update), $"{nameof(Command).ToLower()}:{FormatInheritance(Action.Update)}"));

                if (_action.HasFlag(Action.Delete))
                    @class = @class.AddMembers(Class.WithAsyncNotificationMethod<Guid>("Handle", FormatNotification(Action.Delete), $"{nameof(Command).ToLower()}:{FormatInheritance(Action.Delete)}"));

                return Namespace.AddUsings(System, MediatR, Threading, Tasks)
                        .AddMembers(@class)
                        .NormalizeWhitespace()
                        .ToFullString();
            }
        }

        private string FormatInheritance(Action action)
        {
            return $"{action}{_name.Replace(nameof(CommandHandler), string.Empty)}{nameof(Command)}";
        }

        private string FormatNotification(Action action)
        {
            return $"{_name.Replace(nameof(CommandHandler), string.Empty)}{action}d{nameof(Notification)}";
        }

        private NamespaceDeclarationSyntax Namespace => SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_namespace)).NormalizeWhitespace();
        private UsingDirectiveSyntax System => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(nameof(System)));
        private UsingDirectiveSyntax MediatR => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(nameof(MediatR)));
        private UsingDirectiveSyntax Tasks => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading.Tasks"));
        private UsingDirectiveSyntax Threading => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading"));
        private ClassDeclarationSyntax Class => SyntaxFactory.ClassDeclaration(_name).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
    }
}
