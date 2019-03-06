using System;
using System.Linq;

namespace Hanoog
{
    internal static class ArgumentParser
    {
        public static string ParseProject(string[] args) =>
            args.Any(o => o.Contains("-Project")) ?
            args.FirstOrDefault(o => o.Contains("-Project"))
            .Replace("-Project", string.Empty).Trim() : string.Empty;

        public static Scaffold ParseScaffold(string[] args) =>
            args.Any(o => o.Contains("-Query")) ? Scaffold.Query
                : args.Any(o => o.Contains("-Command")) ? Scaffold.Command
                : args.Any(o => o.Contains("-Notification")) ? Scaffold.Notification
                : Scaffold.All;

        public static Crud ParseAction(string[] args)
        {
            string action = args.FirstOrDefault(o => o.Contains("-Action")).Replace("-Action", string.Empty).Trim();
            Crud flags = Crud.All;

            if (string.IsNullOrEmpty(action))
                return flags;

            if (action.Contains("C"))
                flags = Crud.Create;
            if (action.Contains("U"))
                flags = flags | Crud.Update;
            if (action.Contains("D"))
                flags = flags | Crud.Delete;

            return flags;
        }
    }

    [Flags]
    internal enum Scaffold
    {
        All = 1,
        Query = 2,
        Command = 4,
        Notification = 8
    }

    [Flags]
    internal enum Crud
    {
        All = 1,
        Create = 2,
        Update = 4,
        Delete = 8
    }
}
