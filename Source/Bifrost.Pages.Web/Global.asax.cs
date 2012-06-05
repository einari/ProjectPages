using Bifrost.Configuration;
using Bifrost.Web;
using Bifrost.Execution;
using Bifrost.Unity;
using Microsoft.Practices.Unity;

namespace BifrostPages
{
	public class Global : BifrostHttpApplication
	{
		public override void OnConfigure (Bifrost.Configuration.IConfigure configure)
		{
			WebConfigurationExtensions.AsSinglePageApplication (configure);
				
			
			base.OnConfigure (configure);
		}
		
		protected override IContainer CreateContainer ()
		{
			var unityContainer = new UnityContainer ();
			var container = new Container (unityContainer);
			return container;
		}
		
		
		public override void OnStarted ()
		{
			try {
				DocumentationContent.Initialize ();
			} catch {
			}
			base.OnStarted ();
		}
	}
}

