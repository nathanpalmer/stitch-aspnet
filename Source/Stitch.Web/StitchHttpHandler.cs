using System.Web;
using Stitch.Compilers;

namespace Stitch.Web
{
    public class StitchHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var package = new Package(context.Server.MapPath("."), new[] { "app" }, new [] { "lib/dep.js" }, "require", new[] { new CoffeeScriptCompiler(),  });
            context.Response.ContentType = "text/javascript";
            context.Response.Write(package.Compile());
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}