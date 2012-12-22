using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonSearch.LocaleData
{
    public class UkLocale : LocaleDefinition
    {
        public string EndPointURL()
        {
            return "webservices.amazon.co.uk";
        }

        public List<string> SearchIndexValues()
        {
            return new List<string>()
            {
                "All",
                "Apparel",
                "Automotive",
                "Baby",
                "Beauty",
                "Blended",
                "Books",
                "Classical",
                "DVD",
                "Electronics",
                "Grocery",
                "HealthPersonalCare",
                "HomeGarden",
                "Jewelry",
                "KindelStore",
                "Kitchen",
                "Lighting",
                "Marketplace",
                "MP3Downloads",
                "Music",
                "MusicalInstruments",
                "MusicTracks",
                "OfficeProducts",
                "OutdoorLiving",
                "Outlet",
                "Shoes",
                "Software",
                "SoftwareVideoGames",
                "Toys",
                "VHS",
                "Video",
                "VideoGames",
                "Watches"
            };
        }

        public string BaseCurrencyStr()
        {
            return "GBP";
        }
    }
}