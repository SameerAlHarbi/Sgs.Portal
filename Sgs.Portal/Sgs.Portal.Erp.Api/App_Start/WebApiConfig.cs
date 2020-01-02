using Newtonsoft.Json.Serialization;
using Sgs.Portal.Erp.Api.Helpers;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Sgs.Portal.Erp.Api
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            AutofacConfig.Register();

            // clear the supported mediatypes of the xml formatter
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            
            JsonMediaTypeFormatter json = config.Formatters.JsonFormatter;

            json.SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType: "application/json"));
            json.SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType: "application/json-patch+json"));

            //Changing case of json
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //Use Global Route
            config.MapHttpAttributeRoutes(new CentralizedPrefixProvider("api/erp"));

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/sap/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}