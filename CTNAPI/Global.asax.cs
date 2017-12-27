using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CTNAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Headers.AllKeys.Any(h => h.ToLower().Contains("origin")) && Request.HttpMethod == "OPTIONS")
            {
                if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
                { //These headers are handling the "pre-flight" OPTIONS call sent by the browser 
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", HttpContext.Current.Request.Headers["Access-Control-Request-Headers"] ?? "");
                    HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "DAV, content-length, Allow"); HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credential", "true");
                    HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "3600"); HttpContext.Current.Response.End();
                }
            }
        }
    }
                
}
