using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sputiffy
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Sin esto, las llamadas salientes con HttpClient (YouTube API en
            // ApiMusicaController/PodcastController) pueden colgarse negociando
            // el protocolo con Google y terminar en un TaskCanceledException
            // ("Se canceló una tarea") cuando .NET Framework intenta con TLS
            // viejos antes de llegar a TLS 1.2.
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
