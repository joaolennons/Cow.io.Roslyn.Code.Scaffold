using System;
using System.Collections.Generic;
using Coding;
using Hanoog;
using Microsoft.Build.Construction;
using SolutionManager;

namespace Add_Cqrs
{
    public class CommandChain
    {
        private readonly ProjectInSolution _project;
        private readonly IList<ISourceCode> _commands;
        public CommandChain(ProjectInSolution project)
        {
            _project = project;
            _commands = new List<ISourceCode>();
        }

        private CommandChain Chain<T>(params object[] args) where T : ISourceCode
        {
            var command = (T)Activator.CreateInstance(typeof(T), args);
            _commands.Add(command);
            return this;
        }

        public CommandChain ChainEach<T>(bool expression, Templates.Action flags, string context, string name) where T : ISourceCode
        {
            if (expression == false)
                return this;

            foreach (var flag in flags.GetFlags())
                Chain<T>(context, name, flag);

            return this;
        }

        public CommandChain ChainIf<T>(bool expression, params object[] args) where T : ISourceCode
        {
            if (expression == false)
                return this;

            return Chain<T>(args);
        }

        public void Execute()
        {
            foreach (var command in _commands)
                new ProjectManager(_project)
                    .AddDocument(command);
        }
    }
}
