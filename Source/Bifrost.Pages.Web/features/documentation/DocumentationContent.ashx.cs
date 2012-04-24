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

namespace BifrostPages
{
	public class Element
	{
		public string Name { get; set; }
		public string File { get; set; }
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
		static string _structure = null;
		
		public static void Initialize()
		{
			var groups = new List<Group>();
			var groupsAsJson = ShowTree("3b998fe2271450689072c0ed790af360495d173");
			foreach( var groupAsJson in groupsAsJson["tree"].Children() )
			{
				var group = new Group {
					Name = groupAsJson["name"].Value<string>()
				};
				groups.Add (group);
				
				var topics = new List<Topic>();
				var topicsAsJson = ShowTree(groupAsJson["sha"].Value<string>());
				foreach( var topicAsJson in topicsAsJson["tree"].Children () ) 
				{
					var topic = new Topic {
						Name = topicAsJson["name"].Value<string>()
					};
					topics.Add(topic);
					
					var elements = new List<Element>();
					var elementsAsJson = ShowTree(topicAsJson["sha"].Value<string>());
					foreach( var elementAsJson in elementsAsJson["tree"].Children () )
					{
						var fileName = elementAsJson["name"].Value<string>();
						var baseUrl = "https://raw.github.com/dolittlestudios/Bifrost-Pages/master/Source/Bifrost.Pages.Web/";
						var file = string.Format
							("{0}/features/documentation/content/{1}/{2}/{3}",
							 	baseUrl,
							 	group.Name,
							 	topic.Name,
							 	elementAsJson["name"].Value<string>().ToLowerInvariant()
							 	);
						
						
						var element = new Element {
							Name = Path.GetFileNameWithoutExtension(fileName),
							File = file
						};
						elements.Add (element);
					}
					
					topic.Elements = elements.ToArray ();
				}
				
				group.Topics = topics.ToArray ();
			}
			
			var settings = new JsonSerializerSettings()
            {
            	ContractResolver = new CamelCasePropertyNamesContractResolver()			
			};
			_structure = JsonConvert.SerializeObject(groups, settings);
		}
		
		
		static JObject ShowTree(string parent)
		{
			var url = "http://github.com/api/v2/json/tree/show/dolittlestudios/bifrost-pages/"+parent;
			var request = WebRequest.Create (url);
			var response = request.GetResponse();
			using( var stream = response.GetResponseStream() ) 
			{
				var content = new byte[stream.Length];
				stream.Read (content, 0, content.Length);
				var json = System.Text.UTF8Encoding.UTF8.GetString (content);
				var obj = (JObject)JsonConvert.DeserializeObject(json);
				return obj;
			}
			
			return new JObject();
		}
		

		public virtual bool IsReusable {
			get {
				return true;
			}
		}

		
		public virtual void ProcessRequest (HttpContext context)
		{
			context.Response.ContentType = "application/json";
			context.Response.Write (_structure);
		}
	}
}

