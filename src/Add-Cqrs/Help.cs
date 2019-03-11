using System;

namespace Hanoog
{
    internal class Help
    {
        public static string Info =>
        $"{Environment.NewLine}Add-Cqrs command line tool"
        + $"{Environment.NewLine}"
        + $"{Environment.NewLine}Parameters"
        + $"{Environment.NewLine}----------"
        + $"{Environment.NewLine}-Project - Destination project of generated scaffolds"
        + $"{Environment.NewLine}-Query - Generates QueryObjects and QueryHandlers"
        + $"{Environment.NewLine}-Command - Generates Commands and CommandHandlers"
        + $"{Environment.NewLine}-Notification - Generates Notification and NotificationHandlers"
        + $"{Environment.NewLine}-All - Generates All of above"
        + $"{Environment.NewLine}-Action - Command actions that shall be created. C: Create; U: Update; D: Delete; All or Empty: CUD"
        + $"{Environment.NewLine}-Context - Scaffold's namespace base"
        + $"{Environment.NewLine}-Entity - Domain object name that will be used as sufix for generated scaffolds"
        + $"{Environment.NewLine}";

        public static string Mandatory(string parameter) => $"Supply values for the following parameters: {parameter}";
    }
}
