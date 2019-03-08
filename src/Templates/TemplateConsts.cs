namespace Templates
{
    internal static class TemplateConsts
    {
        public static string IRequest(string response) => $"{nameof(IRequest)}<{response}>";
        public static string INotification => nameof(INotification);
        public static string INotificationHandler(string notification) => $"{nameof(INotificationHandler)}<{notification}>";
        public static string IRequestHandler(string request, string response) => $"{nameof(IRequestHandler)}<{request},{response}>";
    }

    internal class Folders
    {
        public static string Queries => nameof(Queries);
        public static string Notifications => nameof(Notifications);
        public static string Commands => nameof(Commands);
    }

    internal class Method
    {
        public static string EmptyAsyncBlockOf<T>() => $"await Task.FromResult(default({typeof(T).Name}));";
        public static string EmptyAsyncBlockOf(string type) => $"await Task.FromResult(default({type}));";
        public static string EmptyAsyncBlock => "await Task.FromResult(0);";

        public class Returns
        {
            public static string Void => Keywords.Void;
            public static string Task => nameof(Task);
            public static string TaskOf(string type) => $"{nameof(Task)}<{type}>";
        }
    }

    internal class Keywords
    {
        public static string Return => nameof(Return).ToLower();
        public static string Void => nameof(Void).ToLower();
    }
}

