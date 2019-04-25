using Hanoog;

namespace System.Linq
{
    public static class ContainsExtensions
    {
        public static bool ContainsIgnoreCase(this string[] args, string value)
        {
            return args.Contains(value, new IgnoreCase());
        }

        public static bool ContainsIgnoreCase(this string text, string value)
        {
            return text.Contains(value, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
