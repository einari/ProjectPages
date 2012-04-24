using System;
using System.Web;
using System.Web.UI;
using System.Net;

namespace BifrostPages
{
	public class ElementLoader : System.Web.IHttpHandler
	{
		
		public virtual bool IsReusable {
			get {
				return true;
			}
		}
		
		public virtual void ProcessRequest (HttpContext context)
		{
			var file = context.Request["file"];
			if( !string.IsNullOrEmpty(file))
			{
				var request = WebRequest.Create (file);
				var response = request.GetResponse();
				var stream = response.GetResponseStream();
				var buffer = new byte[stream.Length];
				stream.Read (buffer,0,buffer.Length);
				var content = System.Text.UTF8Encoding.UTF8.GetString (buffer);
				
				context.Response.ContentType = "text/plain";
				context.Response.Write (content);
			}
		}
	}
}

