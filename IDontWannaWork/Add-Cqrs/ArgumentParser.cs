using System;
using System.Collections.Generic;
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
    }

    [Flags]
    internal enum Scaffold
    {
        All = 1,
        Query = 2,
        Command = 4,
        Notification = 8
    }
}
