using System.Net;
using System.Text;
using System.Web;
using MarkdownSharp;

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
				
				var buffer = new byte[8192];
				var content = new StringBuilder();
				var count = 0;
				do
				{
					count = stream.Read(buffer,0,buffer.Length);
					if( count != 0 )
						content.Append(UTF8Encoding.UTF8.GetString (buffer));
				} while( count > 0 );
				
				
				var markdown = new Markdown();
				var transformed = markdown.Transform(content.ToString ());
				
				context.Response.Charset = "UTF-8";
				context.Response.ContentType = "text/plain";
				context.Response.Write (transformed);
			}
		}
	}
}

