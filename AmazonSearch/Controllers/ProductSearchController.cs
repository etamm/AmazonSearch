using AmazonSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmazonSearch.CurrencyConvertor;

namespace AmazonSearch.Controllers
{
    public class ProductSearchController : Controller
    {
       
        //
        // GET: /ProductSearch/

        public ActionResult Index()
        {
            
            ViewBag.SearchIndexValues = Globals.CURRENT_LOCALE.SearchIndexValues();
            return View();
        }

        public ActionResult Search( string searchIndex, string searchKeywords, int pageNr )
        {
            var currencies = Enum.GetNames(typeof(Currency));
            ViewBag.Currencies = currencies;
            ViewBag.CurrentCurrency = Globals.CURRENT_LOCALE.BaseCurrencyStr();
            ViewBag.SearchIndexValues = Globals.CURRENT_LOCALE.SearchIndexValues();
            ViewBag.CurrentPage = pageNr;

            ProductSearch model = new ProductSearch(searchIndex, searchKeywords, pageNr);
            return View( model );
        }

        public double ConversionRate(string fromCurrency, string toCurrency)
        {
            
            CurrencyConvertorSoapClient client = new CurrencyConvertorSoapClient("CurrencyConvertorSoap");
            double rate = client.ConversionRate((Currency)Enum.Parse(typeof(Currency), fromCurrency), (Currency)Enum.Parse(typeof(Currency), toCurrency));

            return rate;
        }

    }
}
