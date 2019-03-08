using System;
using System.Linq;
using Hanoog;
using SolutionManager;
using Templates;

namespace Add_Cqrs
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            args = new string[] { "-Project AppTest", "-All", "-Action CUD", "-Context Beer", "-Name Heineken" };

            var projects = SolutionProvider.GetProjects();

            var projectName = ArgumentParser.ParseProject(args);
            var scaffold = ArgumentParser.ParseScaffold(args);
            var crud = ArgumentParser.ParseAction(args);
            var context = ArgumentParser.ParseContext(args);
            var name = ArgumentParser.ParseName(args);

            if (string.IsNullOrEmpty(projectName))
                return;

            if (string.IsNullOrEmpty(context))
                return;

            if (string.IsNullOrEmpty(name))
                return;

            var project = projects.FirstOrDefault(o => o.ProjectName == projectName);

            if (project == null)
            {
                Console.WriteLine($"Projeto {projectName} não existe na solution");
                return;
            }

            new CommandChain(project)
                .ChainIf<QueryObject>(scaffold.HasFlag(Scaffold.Query), context, name)
                .ChainIf<QueryHandler>(scaffold.HasFlag(Scaffold.Query), context, name)
                .ChainEach<Notification>(scaffold.HasFlag(Scaffold.Notification), crud, context, name)
                .ChainIf<NotificationHandler>(scaffold.HasFlag(Scaffold.Notification), context, name, crud)
                .ChainEach<Command>(scaffold.HasFlag(Scaffold.Command), crud, context, name)
                .ChainIf<CommandHandler>(scaffold.HasFlag(Scaffold.Command), context, name, crud)
                .Execute();

            Console.WriteLine("Aperte uma tecla para sair.");
            Console.Read();
        }
    }
}
