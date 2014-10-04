using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Web.Content
{
	public class ImageHandler : IHttpHandler
	{
		
		public virtual bool IsReusable {
			get {
				return false;
			}
		}
		
		public virtual void ProcessRequest (HttpContext context)
		{
			var file = context.Request["file"];
			if( !string.IsNullOrEmpty(file))
			{
				context.Response.ContentType = "image/png";
				var actualFile = context.Server.MapPath(string.Format ("~/App_Data/Repositories/{0}",file));
				if( File.Exists(actualFile)) 
				{
					var image = Bitmap.FromFile(actualFile);
					image.Save (context.Response.OutputStream, ImageFormat.Png);
					context.Response.Flush ();
				}
			}
		}
	}
}

