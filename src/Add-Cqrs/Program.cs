using Hanoog;
using SolutionManager;
using System;
using System.Linq;
using Templates;

namespace Add_Cqrs
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //args = new string[] { "-Project AppTest", "-Context Teste", "-Entity Entity" };
            if (args.ContainsIgnoreCase("-Help"))
            {
                Console.Write(Help.Info);
                return;
            }

            foreach (var param in args)
                Console.WriteLine(param);

            var projects = SolutionProvider.GetProjects();

            var projectName = ArgumentParser.ParseProject(args);
            var scaffold = ArgumentParser.ParseScaffold(args);
            var crud = ArgumentParser.ParseAction(args);
            var context = ArgumentParser.ParseContext(args);
            var entity = ArgumentParser.ParseEntity(args);

            if (string.IsNullOrEmpty(projectName))
            {
                Console.WriteLine(Help.Mandatory("-Project"));
                return;
            }

            if (string.IsNullOrEmpty(context))
            {
                Console.WriteLine(Help.Mandatory("-Context"));
                return;
            }

            if (string.IsNullOrEmpty(entity))
            {
                Console.WriteLine(Help.Mandatory("-Entity"));
                return;
            }

            var project = projects.FirstOrDefault(o => o.ProjectName == projectName);

            if (project == null)
            {
                Console.WriteLine($"Projeto {projectName} não existe na solution");
                return;
            }

            new CommandChain(project)
                .ChainIf<QueryObject>(scaffold.HasFlag(Scaffold.Query), context, entity)
                .ChainIf<QueryHandler>(scaffold.HasFlag(Scaffold.Query), context, entity)
                .ChainEach<Notification>(scaffold.HasFlag(Scaffold.Notification), crud, context, entity)
                .ChainIf<NotificationHandler>(scaffold.HasFlag(Scaffold.Notification), context, entity, crud)
                .ChainEach<Command>(scaffold.HasFlag(Scaffold.Command), crud, context, entity)
                .ChainIf<CommandHandler>(scaffold.HasFlag(Scaffold.Command), context, entity, crud)
                .Execute();

            Console.WriteLine("Aperte uma tecla para sair.");
            Console.Read();
        }
    }
}
