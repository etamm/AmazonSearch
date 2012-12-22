using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AmazonSearch
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Search",
                url: "ProductSearch/Search/{searchIndex}/{searchKeywords}/{pageNr}",
                defaults: new { controller = "ProductSearch", action = "Search", searchIndex = "All", searchKeywords = "", pageNr = 0 }
                );
            routes.MapRoute(
                name: "ConversionRate",
                url: "ProductSearch/ConversionRate/{fromCurrency}/{toCurrency}",
                defaults: new { controller = "ProductSearch", action = "ConversionRate", fromCurrency = Globals.CURRENT_LOCALE.BaseCurrencyStr(), toCurrency = "EURO" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "ProductSearch", action = "Index" }
            );
        }
    }
}