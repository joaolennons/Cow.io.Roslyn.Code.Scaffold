namespace Templates
{
    internal static class TemplateConsts
    {
        public static string IRequest(string response) => $"{nameof(IRequest)}<{response}>";
        public static string INotification => nameof(INotification);
        public static string INotificationHandler(string notification) => $"{nameof(INotificationHandler)}<{notification}>";
        public static string ICommandHandler(string request, string response) => $"{nameof(ICommandHandler)}<{request},{response}>";
    }

    public class Folders
    {
        public static string Queries => nameof(Queries);
        public static string Notifications => nameof(Notifications);
        public static string Commands => nameof(Commands);
    }

    public class Method
    {
        public static string EmptyAsyncBlockOf<T>() => $"await Task.FromResult(default({typeof(T).Name}));";
        public static string EmptyAsyncBlock => "await Task.FromResult(0)";

        public class Returns
        {
            public static string Void => Keywords.Void;
            public static string Task => nameof(Task);
            public static string TaskOf(string type) => $"{nameof(Task)}<{type}>";
        }
    }

    public class Keywords
    {
        public static string Return => nameof(Return).ToLower();
        public static string Void => nameof(Void).ToLower();
    }
}
