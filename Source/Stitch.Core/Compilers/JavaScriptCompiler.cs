using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stitch.Compilers
{
    public class JavaScriptCompiler : ICompile
    {
        public List<string> Extensions { get; private set; }

        public JavaScriptCompiler()
        {
            Extensions = new List<string>(new[] { ".js" });
        }

        public bool Handles(string Extension)
        {
            return Extensions.Where(e => e == Extension).Any();
        }
        
        public string Compile(FileInfo File)
        {
            return System.IO.File.ReadAllText(File.FullName);
        }
    }
}