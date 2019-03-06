using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;

namespace SolutionManager
{
    public class SolutionProvider
    {
        public static string TryGetSolutionDirectoryInfo()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (directory != null && !directory.GetFiles(AppConsts.SolutionExtension).Any())
            {
                directory = directory.Parent;
            }

            return directory.GetFiles(AppConsts.SolutionExtension).First().FullName;
        }

        public static IEnumerable<ProjectInSolution> GetProjects() =>
            SolutionFile.Parse(TryGetSolutionDirectoryInfo()).ProjectsInOrder;
    }
}
