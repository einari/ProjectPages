
using System.Web;
using Web.Features.Documentation;
using System.Collections.Generic;

namespace Web
{
	public class ServerVariables : IHttpHandler
	{
		static List<string> _messages = new List<string>();

		public static void Log(string message, params object[] parameters)
		{
			_messages.Add (string.Format(message, parameters));
		}

		public virtual bool IsReusable {
			get {
				return true;
			}
		}
		
		public virtual void ProcessRequest (HttpContext context)
		{
			context.Response.Charset = "UTF-8";
			context.Response.ContentType = "text/plain";
			context.Response.Write ("ContentPath : "+ContentManager.ContentPath+"\n");
			context.Response.Write ("BifrostPath : "+ContentManager.BifrostPath+"\n");
			context.Response.Write ("DocumentationStructureFile : "+DocumentationContent.FileName+"\n");

			foreach( var message in _messages )
				context.Response.Write ("Message : "+message+"\n");
		}
	}
}

