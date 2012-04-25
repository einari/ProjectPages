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
			var rootSha = GetRootSha();
			var contentSha = rootSha; //GetContentSha(rootSha);
			var groups = new List<Group>();
			var groupsAsJson = ShowTree(contentSha); //"eb70ad92910ff33faf99b1b213a03557b485fbf4"); //3b998fe2271450689072c0ed790af360495d173");
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
						var baseUrl = "https://raw.github.com/dolittlestudios/Bifrost-Documentation/master"; ///Source/Bifrost.Pages.Web/";
						var file = string.Format
							("{0}/{1}/{2}/{3}", // features/documentation/content
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
		
		static string GetShaFromString(string input, string identifier)
		{
			var start = input.IndexOf(identifier)+identifier.Length;
			var end = input.IndexOf("\"",start);
			var sha = input.Substring(start, end-start);
			return sha;
		}
		
	
		static string GetRootSha()
		{
			var url = "http://github.com/api/v2/json/commits/list/dolittlestudios/bifrost-documentation/master";
			var jsonString = GetJsonString(url);
			var sha = GetShaFromString(jsonString,"\"id\":\"");
			return sha;
		}
		
		static JObject ShowTree(string parent)
		{
			var url = "http://github.com/api/v2/json/tree/show/dolittlestudios/bifrost-documentation/"+parent;
			return GetJson(url);
		}
		
		static string GetContentSha(string commitSha)
		{
			var url = "http://github.com/api/v2/json/tree/full/dolittlestudios/bifrost-documentation/"+commitSha;
			var jsonString = GetJsonString (url);
			var sha = GetShaFromString(jsonString, "\"name\":\"Source/Bifrost.Pages.Web/features/documentation/content\",\"size\":0,\"sha\":\"");
			return sha;
		}
		
		
		
		static string GetJsonString(string url)
		{
			var request = WebRequest.Create (url);
			var response = request.GetResponse();
			var stream = response.GetResponseStream();
			
			var buffer = new byte[8192];
			var content = new StringBuilder();
			var count = 0;
			do
			{
				count = stream.Read(buffer,0,buffer.Length);
				if( count != 0 )
					content.Append(UTF8Encoding.UTF8.GetString (buffer));
			} while( count > 0 );

				
			var json = content.ToString();
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
			context.Response.ContentType = "application/json";
			context.Response.Write (_structure);
		}
	}
}

