using System.Collections.Generic;
using System.IO;

namespace Stitch
{
    public interface ICompile
    {
        List<string> Extensions { get; }
        bool Handles(string Extension);
        string Compile(FileInfo File);
    }
}