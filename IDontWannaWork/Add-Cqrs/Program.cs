using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hanoog;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis;
using SolutionManager;
using Templates;

namespace Add_Cqrs
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new string[] { "-Project AppTest", "-All" };

            var projects = SolutionProvider.GetProjects();

            var projectName = ArgumentParser.ParseProject(args);
            var scaffoldKind = ArgumentParser.ParseScaffold(args);

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
                var code = new Notification("Heineken", "BeerCreated");

                new ProjectManager(project)
                    .AddDocument(code);
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
