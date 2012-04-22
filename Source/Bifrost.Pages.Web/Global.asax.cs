using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

//[assembly: WebActivator.PreApplicationStartMethod(typeof(BifrostPages.Global), "Start")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(BifrostPages.Global), "Start")]

namespace BifrostPages
{
	public class Global : System.Web.HttpApplication
	{
		public static void Start()
		{
				var instance = HttpContext.Current.ApplicationInstance;
			var i=0;
			i++;
			//DynamicModuleUtility.RegisterModule()
		}
		
		
		
		protected virtual void Application_Start (Object sender, EventArgs e)
		{
			var i=0;
			i++;
			
		}
		
		protected virtual void Session_Start (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_BeginRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_EndRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_AuthenticateRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_Error (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Session_End (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_End (Object sender, EventArgs e)
		{
		}
	}
}

