using System.IO;
using Coding;
using Microsoft.Build.Construction;

namespace SolutionManager
{
    public class ProjectManager
    {
        private readonly ProjectInSolution _project;
        private readonly string _path;

        public ProjectManager(ProjectInSolution project)
        {
            _project = project;
            _path = project.AbsolutePath.Replace("\\" + project.ProjectName + ".csproj", string.Empty);
        }

        public ProjectManager AddDocument(ISourceCode sourceCode)
        {
            var path = $"{ _path}\\{sourceCode.Folder}";
            System.IO.Directory.CreateDirectory(path);
            File.WriteAllText($"{path}\\{sourceCode.FileName}", sourceCode.Code);
            return this;
        }
    }
}
