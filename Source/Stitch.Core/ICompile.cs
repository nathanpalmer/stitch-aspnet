using System.IO;

namespace Stitch
{
    public interface ICompile
    {
        bool Handles(string Extension);
        string Compile(FileInfo File);
    }
}