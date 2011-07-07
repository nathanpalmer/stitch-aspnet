using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stitch.Compilers
{
    public class jQueryTemplateCompiler : ICompile
    {
        public List<string> Extensions { get; private set; }

        public jQueryTemplateCompiler()
        {
            Extensions = new List<string>(new[] { ".tmpl" });
        }

        public bool Handles(string Extension)
        {
            return Extensions.Where(e => e == Extension).Any();
        }

        public string Compile(FileInfo File)
        {
            var content = System.IO.File.ReadAllText(File.FullName).Replace("\r\n", "").Replace("\n", "").Replace("\"", "'");
            return @"
var template = jQuery.template(""" + content + @""");
module.exports = (function(data){ return jQuery.tmpl(template, data); });";
        }
    }
}

