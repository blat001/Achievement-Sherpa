using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace Web_UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               "Individual Player Parse", // Route name
               "player/parse", // URL with parameters
               new { controller = "PLayer", action = "Parse", region="US" } // Parameter defaults
           );

            routes.MapRoute(
                "Individual Plaer", // Route name
                "player/{region}/{server}/{player}", // URL with parameters
                new { controller = "Player", action = "Index", server = "", player="", region="" } // Parameter defaults
            );
            routes.MapRoute(
            "Guild Ranking", // Route name
            "ranking/{region}/{server}/{guildName}", // URL with parameters
            new { controller = "Ranking", action = "Guild", server = "", region = "", guildName = "" },
            new { guildName = ".+"}// Parameter defaults
        );

            routes.MapRoute(
              "Server Ranking", // Route name
              "ranking/{region}/{server}", // URL with parameters
              new { controller = "Ranking", action = "ServerRanking", server = "", region = "" } // Parameter defaults
          );

            
           
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );



        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            IContainer container = new Container(x =>
            {
                x.AddRegistry<ScanningRegistry>();
            });

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));

        }
    }
}