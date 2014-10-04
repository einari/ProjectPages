using System.Web;
using System.Threading.Tasks;
using Web.Content.Documentation;

namespace Web.Content
{
	public class ResetContent : IHttpHandler
	{
		
		public virtual bool IsReusable {
			get {
				return true;
			}
		}
		
		
		public virtual void ProcessRequest (HttpContext context)
		{
			Parallel.Invoke (() => 
			{
				DocumentationContent.Reset ();
				DocumentationContent.Generate ();
                new ContentManager().Synchronize("Bifrost");
			});

			context.Response.Charset = "UTF-8";
			context.Response.ContentType = "text/plain";
			context.Response.Write ("OK");
		}
	}
}

