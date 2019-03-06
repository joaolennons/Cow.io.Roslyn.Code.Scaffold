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
            args = new string[] { "-Project AppTest", "-All", "-Action All" };

            var projects = SolutionProvider.GetProjects();

            var projectName = ArgumentParser.ParseProject(args);
            var scaffoldKind = ArgumentParser.ParseScaffold(args);
            var crud = ArgumentParser.ParseAction(args);

            if (string.IsNullOrEmpty(projectName))
                return;

            var project = projects.FirstOrDefault(o => o.ProjectName == projectName);

            if (project == null)
            {
                Console.WriteLine($"Projeto {projectName} não existe na solution");
                return;
            }

            if (scaffoldKind.HasFlag(Scaffold.Query) || scaffoldKind.HasFlag(Scaffold.All))
            {
                Console.WriteLine("Gerando QueryObject...");
                var code = new QueryObject("Heineken", "Beer");

                new ProjectManager(project)
                    .AddDocument(code);
            }

            if (scaffoldKind.HasFlag(Scaffold.Notification) || scaffoldKind.HasFlag(Scaffold.All))
            {
                Console.WriteLine("Gerando Notification...");
                if (crud.HasFlag(Crud.Create) || crud.HasFlag(Crud.All))
                {
                    new ProjectManager(project)
                        .AddDocument(new Notification("Heineken", "Beer", Templates.Action.Create));

                    new ProjectManager(project)
                        .AddDocument(new NotificationHandler("Heineken", "Beer", Templates.Action.Create | Templates.Action.Update));
                }
            }

            if (scaffoldKind.HasFlag(Scaffold.Command) || scaffoldKind.HasFlag(Scaffold.All))
            {
                Console.WriteLine("Gerando Command...");
                var code = new Command("Heineken", "Create", "Beer");

                new ProjectManager(project)
                    .AddDocument(code);
            }

            Console.WriteLine("Aperte uma tecla para sair.");
            Console.Read();
        }
    }
}
