using System.Web.Http;
using Swashbuckle.Application;
using System.IO;
using System.Reflection;
using System;
using System.Web;
using Pharos.OMS.Retailing;
[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]
namespace Pharos.OMS.Retailing
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

                        GlobalConfiguration.Configuration 

            .EnableSwagger(c =>
            {
                c.SingleApiVersion(Assembly.GetExecutingAssembly().GetName().Version.ToString(), Assembly.GetExecutingAssembly().GetName().Name);
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.DescribeAllEnumsAsStrings();
            }).EnableSwaggerUi();
        }

        private static string GetXmlCommentsPath()
        {
            var commentsFile = string.Format(@"{1}BIN\{0}.XML", Assembly.GetExecutingAssembly().GetName().Name, AppDomain.CurrentDomain.BaseDirectory);
            return commentsFile;
        }
    }
}
