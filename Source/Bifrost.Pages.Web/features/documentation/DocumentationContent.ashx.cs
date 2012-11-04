using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace BifrostPages
{
	public class Element
	{
		public string Name { get; set; }
		public string File { get; set; }
		public string Author { get; set; }
		public string LastChanged { get; set; }
	}
	
	public class Topic
	{
		public string Name { get; set; }
		public Element[] Elements { get; set; }
	}
	
	public class Group
	{
		public string Name { get; set; }
		public Topic[] Topics { get; set; }
	}
	
	
	public class DocumentationContent : System.Web.IHttpHandler
	{
		static string FileName = "";
		
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
			var rootSha = GetRootSha ();
			var contentSha = rootSha; 
			var groups = new List<Group> ();
			var groupsAsJson = ShowTree (contentSha); //"eb70ad92910ff33faf99b1b213a03557b485fbf4"); //3b998fe2271450689072c0ed790af360495d173");
			foreach (var groupAsJson in groupsAsJson["tree"].Children()) {
				var group = new Group {
					Name = groupAsJson ["path"].Value<string> ()
				};
				if( group.Name.StartsWith(".") )
					continue;

				groups.Add (group);
				
				var topics = new List<Topic> ();
				var topicsAsJson = ShowTree (groupAsJson ["sha"].Value<string> ());
				try {
					foreach (var topicAsJson in topicsAsJson["tree"].Children ()) {
						var topic = new Topic {
							Name = topicAsJson ["path"].Value<string> ()
						};
						topics.Add (topic);
						
						var elements = new List<Element> ();
						var elementsAsJson = ShowTree (topicAsJson ["sha"].Value<string> ());
						foreach (var elementAsJson in elementsAsJson["tree"].Children ()) {
							var fileName = elementAsJson ["path"].Value<string> ();
							var baseUrl = "https://raw.github.com/dolittle/Bifrost-Documentation/master"; ///Source/Bifrost.Pages.Web/";
							var file = string.Format
								("{0}/{1}/{2}/{3}", // features/documentation/content
								 	baseUrl,
								 	group.Name,
								 	topic.Name,
								 	fileName
							);
							

#if(false)
							var commitUrl = "https://api.github.com/repos/dolittle/bifrost-documentation/commits/"+elementAsJson["sha"].Value<string>();
							
							// https://api.github.com/repos/dolittle/bifrost-documentation/commits/508ab7c6817e974def40cc2a3b1b42a2fd99dd15
							// https://api.github.com/repos/dolittle/bifrost-documentation/commits/6e1bb6087c82b063d3c1173072b8edecac57a453
							/*
								"http://github.com/api/v2/json/commits/list/dolittle/bifrost-documentation/master/" +
								group.Name + "/" + topic.Name + "/" + fileName;
							*/
							
							var commitsAsJson = GetJson (commitUrl);
							
							var lastCommit = commitsAsJson ["commits"].Children ().First ();
							var date = DateTime.Parse (lastCommit ["committed_date"].Value<string> ());
							var authorName = lastCommit ["author"] ["name"].Value<string> ();
							var committedDate = string.Format ("{0} - {1}",
									date.ToLongDateString (),
							        date.ToString ("HH:mm"));
			
#else						
							var authorName = "Unknown";
							var committedDate = string.Format ("{0} - {1}",
									DateTime.Now.ToLongDateString (),
							        DateTime.Now.ToString ("HH:mm"));
#endif
							var element = new Element {
								Name = Path.GetFileNameWithoutExtension (fileName),
								File = file,
								Author = authorName,
								LastChanged = committedDate
							};
							elements.Add (element);
						}
						
						topic.Elements = elements.OrderBy (e => e.Name).ToArray ();
					}
				} catch {}
				
				group.Topics = topics.OrderBy (e => e.Name).ToArray ();
			}
			
			var settings = new JsonSerializerSettings ()
            {
            	ContractResolver = new CamelCasePropertyNamesContractResolver ()			
			};
			_structure = JsonConvert.SerializeObject (groups, settings);
			
			Save ();
		}
		
		static string GetShaFromString (string input, string identifier)
		{
			var start = input.IndexOf(identifier)+identifier.Length;
			var end = input.IndexOf("\"",start);
			var sha = input.Substring(start, end-start);
			return sha;
		}
		
	
		static string GetRootSha()
		{
			var commitsUrl = "https://api.github.com/repos/dolittle/bifrost-documentation/commits";
			var jsonString = GetJsonString(commitsUrl);
			var sha = GetShaFromString(jsonString,"\"sha\":\"");
			return sha;
		}
		
		static JObject ShowTree(string parent)
		{
			var url = "https://api.github.com/repos/dolittle/bifrost-documentation/git/trees/"+parent; //+"?recursive=1";
			var json = GetJson(url);
			return json;
		}
		
		
		static string GetJsonString (string url)
		{
			var client = new WebClient ();
			var json = client.DownloadString (new Uri (url));
			return json;
		}
		
		
		static JObject GetJson(string url)
		{
			var json = GetJsonString (url);
			var obj = (JObject)JsonConvert.DeserializeObject(json);
			return obj;
			
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

