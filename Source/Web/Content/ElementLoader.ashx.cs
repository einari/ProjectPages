using System.Web;
using MarkdownSharp;
using System;

namespace Web.Content
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
				var contentManager = new ContentManager();
				var content = contentManager.GetFileContentFor("Bifrost", file);

				var markdown = new Markdown();
				var transformed = markdown.Transform(content);

				file = file.Replace ('\\','/');
				
				var prefix = string.Format("/Content/ImageHandler.ashx?file=Bifrost-Site/{0}",file.Substring(0,file.LastIndexOf("/")+1));
				transformed = transformed.Replace ("<img src=\"","<img src=\""+prefix);
				
				context.Response.Charset = "UTF-8";
				context.Response.ContentType = "text/plain";
				context.Response.Write (transformed);
			}
		}
	}
}