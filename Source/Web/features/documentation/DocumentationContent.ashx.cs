using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace Web.Features.Documentation
{
	public class DocumentationContent : IHttpHandler
	{
		public static string FileName = "";
		static string _structure = null;
		
		public static void Initialize ()
		{
			FileName = HttpContext.Current.Server.MapPath ("~/App_Data/Structure.json");
			
			if (_structure == null) 
				Load ();
			
			if (_structure == null)
				Generate ();
		}
		
	
		static void Load ()
		{
			if (File.Exists (FileName))
				_structure = File.ReadAllText (FileName);
			
		}
		
		static void Save ()
		{
			File.WriteAllText (FileName, _structure);
		}
		
		public static void Generate ()
		{
			var contentManager = new ContentManager();
			contentManager.Synchronize("Bifrost");
			var groups = contentManager.GetStructure("Bifrost");

			var settings = new JsonSerializerSettings ()
            {
            	ContractResolver = new CamelCasePropertyNamesContractResolver ()			
			};
			_structure = JsonConvert.SerializeObject (groups, settings);
			
			Save ();
		}
		


		public virtual bool IsReusable {
			get {
				return true;
			}
		}

		
		public virtual void ProcessRequest (HttpContext context)
		{
            if (_structure == null)
                Initialize();

			context.Response.ContentType = "application/json";
			context.Response.Write (_structure);
		}
	}
}

