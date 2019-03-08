using System.Collections.Generic;
using Coding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Templates
{
    public class NotificationHandler : ISourceCode
    {
        private readonly string _namespace;
        private readonly string _name;
        private readonly Action _action;

        public string FileName => $"{_name}.cs";
        public string Folder => Folders.Notifications;

        public NotificationHandler(string @namespace, string name, Action action)
        {
            _action = action;
            _namespace = @namespace;
            _name = $"{name}{nameof(NotificationHandler)}";
        }

        public string Code
        {
            get
            {
                var inheritances = new List<string>();
                if (_action.HasFlag(Action.Create))
                    inheritances.Add(TemplateConsts.INotificationHandler(FormatInheritance(Action.Create)));
                if (_action.HasFlag(Action.Update))
                    inheritances.Add(TemplateConsts.INotificationHandler(FormatInheritance(Action.Update)));
                if (_action.HasFlag(Action.Delete))
                    inheritances.Add(TemplateConsts.INotificationHandler(FormatInheritance(Action.Delete)));

                var @class = Class.Inherits(inheritances.ToArray());

                if (_action.HasFlag(Action.Create))
                    @class = @class.AddMembers(Class.WithAsyncMethod("Handle", parameters: $"{nameof(Notification).ToLower()}:{FormatInheritance(Action.Create)}"));

                if (_action.HasFlag(Action.Update))
                    @class = @class.AddMembers(Class.WithAsyncMethod("Handle", parameters: $"{nameof(Notification).ToLower()}:{FormatInheritance(Action.Update)}"));

                if (_action.HasFlag(Action.Delete))
                    @class = @class.AddMembers(Class.WithAsyncMethod("Handle", parameters: $"{nameof(Notification).ToLower()}:{FormatInheritance(Action.Delete)}"));

                return Namespace.AddUsings(System, MediatR, Threading, Tasks)
                        .AddMembers(@class)
                        .NormalizeWhitespace()
                        .ToFullString();
            }
        }

        private string FormatInheritance(Action action)
        {
            return $"{_name.Replace(nameof(NotificationHandler), string.Empty)}{action.ToPastForm()}{nameof(Notification)}";
        }

        private NamespaceDeclarationSyntax Namespace => SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_namespace)).NormalizeWhitespace();
        private UsingDirectiveSyntax System => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(nameof(System)));
        private UsingDirectiveSyntax MediatR => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(nameof(MediatR)));
        private UsingDirectiveSyntax Tasks => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading.Tasks"));
        private UsingDirectiveSyntax Threading => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading"));
        private ClassDeclarationSyntax Class => SyntaxFactory.ClassDeclaration(_name).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
    }
}
