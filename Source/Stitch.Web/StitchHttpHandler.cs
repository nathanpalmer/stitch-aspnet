using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;
using Stitch.Compilers;

namespace Stitch.Web
{
    public class StitchHttpHandler : IHttpHandler
    {
        private static StitchConfigurationSection.StitchConfiguration configuration;
        private static List<ICompile> compilers;

        static StitchHttpHandler()
        {
            configuration = (StitchConfigurationSection.StitchConfiguration)ConfigurationManager.GetSection("stitch");

            compilers = new List<ICompile>();
            foreach(var compiler in configuration.Compilers)
            {
                compilers.Add((ICompile) Activator.CreateInstance(Type.GetType(compiler.Type)));
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var package = new Package(context.Server.MapPath("."), configuration.Paths, configuration.Dependencies, configuration.Identifier ?? "require", compilers);
            context.Response.ContentType = "text/javascript";
            context.Response.Write(package.Compile());
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}