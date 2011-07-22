using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Stitch.Web
{
    public class StitchHttpHandler : IHttpHandler
    {
        private static readonly StitchConfiguration configuration;
        private static readonly List<ICompile> compilers;
        private static Exception exception;

        static StitchHttpHandler()
        {
            try
            {
                configuration = (StitchConfiguration) ConfigurationManager.GetSection("stitch");

                compilers = new List<ICompile>();
                foreach (var compiler in configuration.Compilers)
                {
                    compilers.Add((ICompile) Activator.CreateInstance(Type.GetType(compiler.Type)));
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (exception != null) throw exception;

                Package package = null;
                if (configuration.Files != null)
                {
                    var file = configuration.Files.Where(f => f.Name.Equals(context.Request.Path, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (file != null)
                    {
                        package = new Package(
                            context.Server.MapPath("."),
                            file.Paths,
                            file.Dependencies,
                            file.Identifier ?? configuration.Identifier ?? "require",
                            compilers);
                    }
                }
                
                if (package == null)
                {
                    package = new Package(
                        context.Server.MapPath("."),
                        configuration.Paths,
                        configuration.Dependencies,
                        configuration.Identifier ?? "require",
                        compilers);
                }

                context.Response.ContentType = "application/x-javascript";
                var content = package.Compile();
                context.Response.AddHeader("Content-Length", content.Length.ToString());
                context.Response.Write(content);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("EXCEPTION: " + ex);
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}