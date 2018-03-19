using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace ViewHound.Mvc5
{
    /// <summary>
    /// Wrapper around Directory static class 
    /// </summary>
    public class DirectoryHelper : IDirectoryHelper
    {
        public string[] GetFiles(string path, string pattern, SearchOption searchOption)
        {

            var paths = Directory.GetFiles(path, pattern, searchOption).Select(f => GetRelativePath(f, HostingEnvironment.ApplicationPhysicalPath)).ToArray();
            return paths;

        }

        private static string GetRelativePath(string filespec, string folder)
        {
            var pathUri = new Uri(filespec);
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            var folderUri = new Uri(folder);
            return $"~/{Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString())}";
        }
    }
}