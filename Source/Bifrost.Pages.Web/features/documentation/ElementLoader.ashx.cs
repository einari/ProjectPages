using System;
using System.Web;
using System.Web.UI;

namespace BifrostPages
{
	public class ElementLoader : System.Web.IHttpHandler
	{
		
		public virtual bool IsReusable {
			get {
				return false;
			}
		}
		
		public virtual void ProcessRequest (HttpContext context)
		{
		}
	}
}

