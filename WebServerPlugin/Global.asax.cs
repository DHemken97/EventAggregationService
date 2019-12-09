using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using EAS_Development_Interfaces;

namespace WebServerPlugin
{
    public class Global : HttpApplication,IService
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Start()
        {
            Application_Start(this,null);
        }

        public void Stop()
        {
        }
    }
}