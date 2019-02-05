using System.Web.Http;
using System.Xml.Linq;
using System.Xml.XPath;
using Api;
using Swashbuckle.Application;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Notification");
                    c.IncludeXmlComments(GetXmlCommentsPath());
                    c.DescribeAllEnumsAsStrings();

                    c.GroupActionsBy(apiDesc =>
                    {
                        var controllerName = apiDesc.ActionDescriptor.ControllerDescriptor.ControllerName;
                        var controllerType = apiDesc.ActionDescriptor.ControllerDescriptor.ControllerType.ToString();
                        var member = XDocument.Load(GetXmlCommentsPath()).Root?.XPathSelectElement($"/doc/members/member[@name=\"T:{controllerType}\"]");
                        return $"{controllerName} : {member?.XPathSelectElement("summary")?.Value}";
                    });

                })
               .EnableSwaggerUi(c => { });
        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\Swagger.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
