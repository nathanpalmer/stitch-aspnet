using System;
using System.Collections.Generic;
using Stitch.Compilers;

namespace Stitch
{
    class Settings
    {
        public string[] Paths { get; set; }
        public string[] Dependencies { get; set; }
        public string Identifier { get; set; }
        public ICompile[] Compilers { get; set; }
        public string Root { get; set; }

        public Settings()
        {
            Root = Environment.CurrentDirectory;
            Identifier = "require";
            Compilers = new ICompile[] {new CoffeeScriptCompiler(), new JavaScriptCompiler()};
            Dependencies = new string[0];
        }
    }
}