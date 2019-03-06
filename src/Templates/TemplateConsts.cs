using System;
using System.Collections.Generic;
using System.Text;

namespace Templates
{
    internal static class TemplateConsts
    {
        public static string IRequest(string response) => $"{nameof(IRequest)}<{response}>";
        public static string INotification => nameof(INotification);
    }

    public class Folders
    {
        public static string Queries => nameof(Queries);
        public static string Notifications => nameof(Notifications);
        public static string Commands => nameof(Commands);
    }
}
