using Bifrost.Configuration;
using Bifrost.Web;
using Bifrost.Execution;
using Bifrost.Unity;
using Microsoft.Practices.Unity;
using Web.Features.Documentation;
using System;

namespace Web
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
				ContentManager.Initialize(Server);
				DocumentationContent.Initialize (Server);
			} catch( Exception ex) {
				ServerVariables.Log ("Exception : {0}, {1}", ex.Message, ex.StackTrace);
			}
			base.OnStarted ();
		}
	}
}

