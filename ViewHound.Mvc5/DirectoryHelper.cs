using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Jumanji.Framework.ViewTracker
{
    public class DirectoryHelper : IDirectoryHelper
    {
        public string[] GetFiles(string path, string pattern, SearchOption searchOption)
        {

            var paths = Directory.GetFiles(path, pattern, searchOption).Select(f => GetRelativePath(f, HostingEnvironment.ApplicationPhysicalPath)).ToArray();
            return paths;

        }
        string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return $"~/{Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString())}";
        }
    }
}