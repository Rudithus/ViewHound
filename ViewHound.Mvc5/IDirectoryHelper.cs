using System.IO;

namespace Jumanji.Framework.ViewTracker
{
    public interface IDirectoryHelper
    {
        string[] GetFiles(string path, string pattern, SearchOption searchOption);
    }
}