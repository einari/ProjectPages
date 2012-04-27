using System.Web;
using System.Threading.Tasks;

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
			Parallel.Invoke (() => DocumentationContent.Generate ());
		}
	}
}

