using System.IO;

namespace Stitch.Compilers
{
    class JavaScriptCompiler : ICompile
    {
        public bool Handles(string Extension)
        {
            return Extension == ".js";
        }

        public string Compile(FileInfo File)
        {
            return System.IO.File.ReadAllText(File.FullName);
        }
    }
}