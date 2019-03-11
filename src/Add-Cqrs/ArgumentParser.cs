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

        public static Scaffold ParseScaffold(string[] args)
        {
            bool all = args.Any(o => o.Contains("-All"));
            bool query = args.Any(o => o.Contains("-Query"));
            bool notification = args.Any(o => o.Contains("-Notification"));
            bool command = args.Any(o => o.Contains("-Command"));

            var flags = Scaffold.None;

            if (query)
                flags = Scaffold.Query;

            if (notification)
                flags = flags | Scaffold.Notification;

            if (command)
                flags = flags | Scaffold.Command;

            if (all || flags == Scaffold.None)
                return Scaffold.Query | Scaffold.Notification | Scaffold.Command;

            return flags;
        }

        internal static string ParseEntity(string[] args) =>
            args.Any(o => o.Contains("-Entity")) ?
                args.FirstOrDefault(o => o.Contains("-Entity"))
                .Replace("-Entity", string.Empty).Trim() : string.Empty;

        internal static string ParseContext(string[] args) =>
            args.Any(o => o.Contains("-Context")) ?
                    args.FirstOrDefault(o => o.Contains("-Context"))
                    .Replace("-Context", string.Empty).Trim() : string.Empty;

        public static Templates.Action ParseAction(string[] args)
        {
            string action = args.Any(o => o.Contains("-Action")) ? args.FirstOrDefault(o => o.Contains("-Action")).Replace("-Action", string.Empty).Trim() : string.Empty;
            var flags = Templates.Action.None;

            if (string.IsNullOrEmpty(action) || action == "All")
                return Templates.Action.Create | Templates.Action.Update | Templates.Action.Delete;

            if (action.Contains("C"))
                flags = Templates.Action.Create;
            if (action.Contains("U"))
                flags = flags | Templates.Action.Update;
            if (action.Contains("D"))
                flags = flags | Templates.Action.Delete;

            return flags;
        }
    }

    [Flags]
    internal enum Scaffold
    {
        Query = 1,
        Command = 2,
        Notification = 4,
        None = 8
    }
}
