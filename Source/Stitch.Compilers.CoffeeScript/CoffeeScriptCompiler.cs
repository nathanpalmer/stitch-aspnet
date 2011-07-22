using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SassAndCoffee.Core;
using SassAndCoffee.Core.Caching;
using SassAndCoffee.Core.Compilers;

namespace Stitch.Compilers
{
    public class CoffeeScriptCompiler : ICompile
    {
        private ContentCompiler csCompiler;
        public List<string> Extensions { get; private set; }

        public CoffeeScriptCompiler()
        {
            Extensions = new List<string>(new[] { ".coffee" });
            csCompiler = new ContentCompiler(new NoRootCompilerHost(), new InMemoryCache(), new[] { new CoffeeScriptFileCompiler(new SassAndCoffee.Core.Compilers.CoffeeScriptCompiler()),  });
        }

        public bool Handles(string Extension)
        {
            return Extensions.Where(e => e == Extension).Any();
        }

        public string Compile(FileInfo File)
        {
            var outputFile = File.FullName.ToLowerInvariant().Replace(".coffee", ".js");
            var result = csCompiler.GetCompiledContent(outputFile);
            if (result.Compiled)
            {
                return result.Contents;
            }
            throw new Exception("Unable to compile coffeescript");
        }

        public class NoRootCompilerHost : ICompilerHost
        {
            public string MapPath(string path)
            {
                return path;
            }

            public string ApplicationBasePath
            {
                get
                {
                    return "";
                }
            }
        }
    }
}