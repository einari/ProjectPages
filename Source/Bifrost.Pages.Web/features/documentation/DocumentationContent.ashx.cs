using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace BifrostPages
{
	public class Directory
	{
		public string Path { get; set; }
		
		public Directory[] Directories { get; set; }
	}
	
	
	public class DocumentationContent : System.Web.IHttpHandler
	{
		
		public virtual bool IsReusable {
			get {
				return false;
			}
		}
		
		public virtual void ProcessRequest (HttpContext context)
		{
			var folder = context.Request.MapPath ("~/features/documentation/content");
			
			var files =
				Directory.GetFiles (folder, "*.txt", SearchOption.AllDirectories)
					.Select (d => d.Replace (folder, string.Empty))
					.OrderBy (d => d);
			
			foreach (var file in files) {
				
			
			}
			
			
			
			
			
			
			
		}
	}
}

