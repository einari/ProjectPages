using System;
using System.Web;
using System.Web.UI;

namespace BifrostPages
{
	public class ResetContent : System.Web.IHttpHandler
	{
		
		public virtual bool IsReusable {
			get {
				return true;
			}
		}
		
		public virtual void ProcessRequest (HttpContext context)
		{
			DocumentationContent.Initialize();
		}
	}
}

