namespace Templates
{
    internal static class TemplateConsts
    {
        public static string IRequest(string response) => $"{nameof(IRequest)}<{response}>";
        public static string INotification => nameof(INotification);
        public static string INotificationHandler(string notification) => $"{nameof(INotificationHandler)}<{notification}>";
    }

    public class Folders
    {
        public static string Queries => nameof(Queries);
        public static string Notifications => nameof(Notifications);
        public static string Commands => nameof(Commands);
    }

    public class Method
    {
        public static string EmptyAsyncBlock => "return await Task.FromResult(0);";

        public class Returns
        {
            public static string Void => nameof(Void);
            public static string Task => nameof(Task);
        }
    }
}
