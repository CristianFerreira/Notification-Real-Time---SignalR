using System.Web.Http;
using System.Web.Http.Cors;
using ExceptionHelper.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            #region Exceptions
            config.Filters.Add(new ExceptionFilter());
            //config.Filters.Add(new LogFilter());
            #endregion

            #region Web API JSON formatters
            // Web API JSON formatters configuration
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Formatting.Indented;
            json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
            #endregion

            #region Web API configuration CORS

            // Web API configuration CORS
            config.EnableCors(new EnableCorsAttribute("*", "*", "GET,POST,PUT,DELETE"));

            #endregion

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
