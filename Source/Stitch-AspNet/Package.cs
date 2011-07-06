using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Stitch_AspNet
{
    public class Package
    {
        public string[] Paths { get; set; }
        public string Identifier { get; set; }
        protected string Root { get; set; }
        protected List<ICompile> Compilers { get; set; }

        public Package(string Root, string[] Paths, string Identifier = "require")
        {
            this.Identifier = Identifier;
            Compilers = new List<ICompile>(new[] { new CoffeeScriptCompiler() });
            this.Root = Root.ToLower();
            this.Paths = Paths;
        }

        public string Compile()
        {
            var sw = new StringWriter();
            sw.Write(@"
(function(/*! Stitch !*/) {
  if (!this." + Identifier + @") {
    var modules = {}, cache = {}, require = function(name, root) {
      var module = cache[name], path = expand(root, name), fn;
      if (module) {
        return module;
      } else if (fn = modules[path] || modules[path = expand(path, './index')]) {
        module = {id: name, exports: {}};
        try {
          cache[name] = module.exports;
          fn(module.exports, function(name) {
            return require(name, dirname(path));
          }, module);
          return cache[name] = module.exports;
        } catch (err) {
          delete cache[name];
          throw err;
        }
      } else {
        throw 'module \'' + name + '\' not found';
      }
    }, expand = function(root, name) {
      var results = [], parts, part;
      if (/^\.\.?(\/|$)/.test(name)) {
        parts = [root, name].join('/').split('/');
      } else {
        parts = name.split('/');
      }
      for (var i = 0, length = parts.length; i < length; i++) {
        part = parts[i];
        if (part == '..') {
          results.pop();
        } else if (part != '.' && part != '') {
          results.push(part);
        }
      }
      return results.join('/');
    }, dirname = function(path) {
      return path.split('/').slice(0, -1).join('/');
    };
    this." + Identifier + @" = function(name) {
      return require(name, '');
    }
    this." + Identifier + @".define = function(bundle) {
      for (var key in bundle)
        modules[key] = bundle[key];
    };
  }
  return this." + Identifier + @".define;
}).call(this)({
");

            foreach (var path in Paths)
            {
                var rootPath = Path.Combine(Root, path) + "\\";
                var i = 0;
                foreach(var item in GatherSources(new FileInfo(rootPath)))
                {
                    sw.Write(i == 0 ? "" : ", ");
                    sw.Write(string.Format("\"{0}\"", item.FullName.ToLower().Replace(rootPath,"").Replace("\\", "/").Replace(item.Extension,"")));
                    sw.Write(": function(exports, require, module) ");
                    sw.Write("{" + string.Join("", Compilers.Where(c => c.Handles(item.Extension)).Select(c => c.Compile(item))) + "}");

                    i++;
                }
            }

            var items = new List<FileInfo>(Paths.SelectMany(p => GatherSources(new FileInfo(p))));
            for (int i = 0; i < items.Count; i++)
            {
                
            }
            sw.Write("});" + Environment.NewLine);
            return sw.ToString();
        }

        protected IEnumerable<FileInfo> GatherSources(FileSystemInfo Item)
        {
            var dir = new DirectoryInfo(Item.FullName);
            if (dir.Exists)
            {
                var items = dir.GetFileSystemInfos("*").SelectMany(GatherSources);
                foreach(var item in items)
                {
                    yield return item;
                }
            }
            else
            {
                yield return new FileInfo(Item.FullName);
            }
        }
    }
}