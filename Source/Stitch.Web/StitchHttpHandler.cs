using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Stitch.Web
{
    public class StitchHttpHandler : IHttpHandler
    {
        private static readonly StitchConfigurationSection.StitchConfiguration configuration;
        private static readonly List<ICompile> compilers;

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
            var package = new Package(
                context.Server.MapPath("."),
                configuration.Paths,
                configuration.Dependencies,
                configuration.Identifier ?? "require",
                compilers);

            context.Response.ContentType = "text/javascript";
            context.Response.Write(package.Compile());
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}