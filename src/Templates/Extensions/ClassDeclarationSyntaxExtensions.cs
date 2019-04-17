using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Templates
{
    public static class ClassDeclarationSyntaxExtensions
    {
        public static ClassDeclarationSyntax Inherits(this ClassDeclarationSyntax @class, params string[] bases)
        {
            foreach (var @base in bases)
            {
                @class = @class.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(@base)));
            }
            return @class;
        }

        /// <summary>
        /// </summary>
        /// <param name="class"></param>
        /// <param name="name"></param>
        /// <param name="parameters">ex.: p1:string, p2:int</param>
        /// <returns></returns>
        public static MethodDeclarationSyntax WithAsyncMethod(this ClassDeclarationSyntax @class, string name, params string[] parameters)
        {
            var @params = new List<ParameterSyntax>();
            foreach (var param in parameters)
                @params.Add(CreateParameter(param.Split(':')[0], param.Split(':')[1]));

            @params.Add(CreateParameter("cancellationToken", nameof(System.Threading.CancellationToken)));

            return SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(Method.Returns.Task), name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
                .AddParameterListParameters(@params.ToArray())
                .WithBody(SyntaxFactory.Block(SyntaxFactory.ParseStatement(Method.EmptyAsyncBlock)));
        }

        /// <summary>
        /// </summary>
        /// <param name="class"></param>
        /// <param name="name"></param>
        /// <param name="parameters">ex.: p1:string, p2:int</param>
        /// <returns></returns>
        public static MethodDeclarationSyntax WithParameterlessMethod(this ClassDeclarationSyntax @class, string returnType, string name)
        {
            return SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(returnType), name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .WithBody(SyntaxFactory.Block(SyntaxFactory.ParseStatement(Method.Returns.Self(returnType))));
        }

        public static MethodDeclarationSyntax WithAsyncMethod(this ClassDeclarationSyntax @class, string name, string responseType, params string[] parameters)
        {
            var @params = new List<ParameterSyntax>();
            foreach (var param in parameters)
                @params.Add(CreateParameter(param.Split(':')[0], param.Split(':')[1]));

            @params.Add(CreateParameter("cancellationToken", nameof(System.Threading.CancellationToken)));

            return SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(Method.Returns.TaskOf(responseType)), name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
                .AddParameterListParameters(@params.ToArray())
                .WithBody(SyntaxFactory.Block(SyntaxFactory.ParseStatement($"{Keywords.Return} {Method.EmptyAsyncBlockOf(responseType)}")));
        }

        public static MethodDeclarationSyntax WithAsyncMethod<T>(this ClassDeclarationSyntax @class, string name, params string[] parameters)
        {
            var @params = new List<ParameterSyntax>();
            foreach (var param in parameters)
                @params.Add(CreateParameter(param.Split(':')[0], param.Split(':')[1]));

            @params.Add(CreateParameter("cancellationToken", nameof(System.Threading.CancellationToken)));

            return SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(Method.Returns.TaskOf(typeof(T).Name)), name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
                .AddParameterListParameters(@params.ToArray())
                .WithBody(SyntaxFactory.Block(SyntaxFactory.ParseStatement($"{Keywords.Return} {Method.EmptyAsyncBlockOf<T>()}")));
        }

        public static MethodDeclarationSyntax WithAsyncNotificationMethod<T>(this ClassDeclarationSyntax @class, string name, string body, params string[] parameters)
        {
            var @params = new List<ParameterSyntax>();
            foreach (var param in parameters)
                @params.Add(CreateParameter(param.Split(':')[0], param.Split(':')[1]));

            @params.Add(CreateParameter("cancellationToken", nameof(System.Threading.CancellationToken)));

            return SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(Method.Returns.TaskOf(typeof(T).Name)), name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
                .AddParameterListParameters(@params.ToArray())
                .WithBody(SyntaxFactory.Block(
                    SyntaxFactory.ParseStatement($"await _mediator.Publish({body}.Raise());"),
                    SyntaxFactory.ParseStatement($"{Keywords.Return} {Method.EmptyAsyncBlockOf<T>()}")));
        }

        private static ParameterSyntax CreateParameter(string name, string type) =>
            SyntaxFactory.Parameter(SyntaxFactory.Identifier(name))
            .WithType(SyntaxFactory.ParseTypeName(type));
    }
}
