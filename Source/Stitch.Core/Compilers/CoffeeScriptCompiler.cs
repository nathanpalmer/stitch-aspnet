using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoffeeSharp;

namespace Stitch.Compilers
{
    public class CoffeeScriptCompiler : ICompile
    {
        private CoffeeScriptEngine csCompiler;
        public List<string> Extensions { get; private set; }

        public CoffeeScriptCompiler()
        {
            Extensions = new List<string>(new[] { ".coffee" });
            csCompiler = new CoffeeScriptEngine();
        }

        public bool Handles(string Extension)
        {
            return Extensions.Where(e => e == Extension).Any();
        }

        public string Compile(FileInfo File)
        {
            return csCompiler.Compile(System.IO.File.ReadAllText(File.FullName));
        }
    }
}