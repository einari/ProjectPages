using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Web.Content.API
{
    /// <summary>
    /// Summary description for APIStructureHandler
    /// </summary>
    public class APIStructureHandler : IHttpHandler
    {
        public static IContentManager _contentManager = new ContentManager();

        public void ProcessRequest(HttpContext context)
        {
            var assemblies = _contentManager.GetAPIStructure("Bifrost");

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var structure = JsonConvert.SerializeObject(assemblies, settings);

            context.Response.ContentType = "application/json";
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