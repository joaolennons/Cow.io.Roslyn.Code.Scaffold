using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
    }
}
