using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmazonSearch.LocaleData;

namespace AmazonSearch
{
    public static class Globals
    {
        public const String ACCESS_KEY_ID = "";
        public const String SECRET_KEY = "";
        public const String ASSOCIATE_TAG = "";

        public const int AMAZON_API_MAX_PRODUCTS_PER_PAGE = 10;
        public const int AMAZON_API_MAX_PAGES_INDEX_ALL = 5; // Max pages when searchindex = All
        public const int AMAZON_API_MAX_PAGES_INDEX_OTHER = 10; // Max pages when index != All

        public static LocaleDefinition CURRENT_LOCALE = new UkLocale();
        public const int PRODUCTS_PER_PAGE = 13;
       
    }
}