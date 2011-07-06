using System.Web;

namespace Stitch
{
    public class StitchHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var package = new Package(context.Server.MapPath("."), new[] { "app" }, "require", new[] { new CoffeeScriptCompiler(),  });
            context.Response.ContentType = "text/javascript";
            context.Response.Write(package.Compile());
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}