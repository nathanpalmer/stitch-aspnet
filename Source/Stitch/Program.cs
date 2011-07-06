using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Documentation;

namespace Stitch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Stitch Command Line Compiler {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Console.WriteLine("Copyright (C) 2011 Nathan Palmer");
            Console.WriteLine("http://github.com/nathanpalmer/stitch-aspnet");
            Console.WriteLine();

            try
            {
                var options = new Options();
                var configuration = new Settings();
                options.Add("r=|root=", "Root path (default is working directory)", v => configuration.Root = v);
                options.Add("p=|paths=", "Comma delimited list of paths that should be compiled", v => configuration.Paths = v.Split(','));
                options.Add("d=|dep=", "Comma delimited list of dependencies that should be included", v => configuration.Dependencies = v.Split(','));
                options.Add("i=|identifier=", "Identifier to use for including other files (default is require)", v => configuration.Identifier = v);
                options.Add("c=|compilers=", "Comma delimited list of compilers to use (default is CoffeeScriptCompiler, JavaScriptCompiler)", v => configuration.Compilers = v.Split(',').Select(compiler => (ICompile) Activator.CreateInstance(Type.GetType("Stitch.Compilers." + compiler + ", Stitch.Core"))).ToArray());

                if (args.Length == 0)
                {
                    ShowHelp(options, "No arguments specified.");
                }
                else
                {
                    var extra = options.Parse(args).ToArray();
                    if (extra.Length > 1)
                    {
                        Console.WriteLine("The following arguments did not parse.\r\n\r\n" + string.Join(",", extra));
                    }
                    else if (extra.Length == 1)
                    {
                        var file = new FileInfo(extra[0]);
                        if (file.Exists)
                        {
                            file.Delete();
                        }

                        Console.WriteLine("Generating {0}", file.Name);

                        var package = new Package(
                            configuration.Root,
                            configuration.Paths,
                            configuration.Dependencies,
                            configuration.Identifier ?? "require",
                            configuration.Compilers);

                        File.WriteAllText(file.FullName, package.Compile());
                    }
                    else
                    {
                        ShowHelp(options, "You must specify a destination.");
                        return;
                    }

                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal Exception: " + ex.ToString());
            }
        }

        static void ShowHelp(Options options, string message = "")
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(message);
                Console.WriteLine("");
            }
            
            Console.WriteLine("Stitch [destination] options");
            Console.WriteLine("");
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}
