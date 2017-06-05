using System.Web.Http;
using WebActivatorEx;
using SuiBao_v1.WebApi;
using Swashbuckle.Application;
using SuiBao_v1.WebApi.App_Start;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace SuiBao_v1.WebApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration 
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "SuiBao_v1.WebApi");
                        c.IncludeXmlComments(string.Format("{0}/App_Data/SuiBao.WebApi.XML", System.AppDomain.CurrentDomain.BaseDirectory));
                        c.CustomProvider((defaultProvider) => new SwaggerCachingProvider(defaultProvider));
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.InjectJavaScript(thisAssembly, "SuiBao_v1.WebApi.Scripts.swagger_lang.js");
                    });
        }
    }
}
