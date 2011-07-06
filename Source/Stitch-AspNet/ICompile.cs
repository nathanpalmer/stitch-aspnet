using System.IO;

namespace Stitch_AspNet
{
    public interface ICompile
    {
        bool Handles(string Extension);
        string Compile(FileInfo File);
    }
}