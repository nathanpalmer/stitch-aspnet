using System.IO;
using CoffeeSharp;

namespace Stitch.Compilers
{
    public class CoffeeScriptCompiler : ICompile
    {
        private CoffeeScriptEngine csCompiler;

        public CoffeeScriptCompiler()
        {
            csCompiler = new CoffeeScriptEngine();
        }

        public bool Handles(string Extension)
        {
            return Extension == ".coffee";
        }

        public string Compile(FileInfo File)
        {
            return csCompiler.Compile(System.IO.File.ReadAllText(File.FullName));
        }
    }
}