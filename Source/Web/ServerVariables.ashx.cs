
using System.Web;
using Web.Features.Documentation;

namespace Web
{
	public class ServerVariables : IHttpHandler
	{
		
		public virtual bool IsReusable {
			get {
				return true;
			}
		}
		
		public virtual void ProcessRequest (HttpContext context)
		{
			context.Response.Charset = "UTF-8";
			context.Response.ContentType = "text/plain";
			context.Response.Write ("ContentPath : "+ContentManager.ContentPath);
		}
	}
}

