using System.Web;
using System.Threading.Tasks;

namespace Web.Features.Documentation
{
	public class ResetContent : IHttpHandler
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

