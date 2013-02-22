using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Web.Content
{
    /// <summary>
    /// Summary description for Tutorials
    /// </summary>
    public class Tutorials : IHttpHandler
    {
        public static IContentManager _contentManager = new ContentManager();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var tutorials = _contentManager.GetTutorials("Bifrost");

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var structure = JsonConvert.SerializeObject(tutorials, settings);
            context.Response.Write(structure);
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}