using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonSearch.LocaleData
{
    public interface LocaleDefinition
    {
        /*
         * Valid at [December 20th 2012]
         * Endpoint URL should be in the format of: webservices.amazon.*. * - your domain
         * Internally we are using the REST GET request so "/onca/xml" will be appended to the url in SignedRequestHelper.cs.
         * 
         */
        String EndPointURL();

        List<String> SearchIndexValues();
        String BaseCurrencyStr();
    }
}
