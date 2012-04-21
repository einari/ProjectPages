using System;
using System.IO;
using System.Web;

namespace BifrostPages
{
	public class RedirectModule : IHttpModule
	{
		public void Dispose ()
		{
		}

		public void Init (HttpApplication context)
		{
			context.AuthorizeRequest += HandleContextAuthorizeRequest;
		}


		void HandleContextAuthorizeRequest (object sender, EventArgs e)
		{
			var context = HttpContext.Current;
			var path = context.Request.Path;
			if( path.Length > 0 ) 
			{
				if( path.StartsWith("/") ) 
				{
					var extension = Path.GetExtension(path);
					if( string.IsNullOrEmpty(extension) ) 
					{
						context.RewritePath("/index.html");
					}
				}
			}
		}
	}
}

