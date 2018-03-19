using System.IO;

namespace ViewHound.Mvc5
{
    public interface IDirectoryHelper
    {
        string[] GetFiles(string path, string pattern, SearchOption searchOption);
    }
}