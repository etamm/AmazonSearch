using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmazonProductAdvtApi;
using System.ServiceModel;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using System.Threading;

namespace AmazonSearch.Models
{
    public class ProductSearch
    {
        SignedRequestHelper helper = new SignedRequestHelper(Globals.ACCESS_KEY_ID, Globals.SECRET_KEY, Globals.CURRENT_LOCALE.EndPointURL());

        public ProductSearch(string searchIndex, string keywords, int pageNr)
        {
            errorOnSearch_ = false;
            errorMessage_ = "";
            searchIndex_ = searchIndex;
            searchKeywords_ = keywords;
            if (keywords.Length == 0)
            {
                errorOnSearch_ = true;
                errorMessage_ = "Please enter a search term";
                return;
            }
            int firstProductIndex = pageNr * Globals.PRODUCTS_PER_PAGE;
            int lastProductIndex = firstProductIndex + Globals.PRODUCTS_PER_PAGE - 1;

            totalProducts_ = FoundProductsCount(searchIndex, keywords);

            if (totalProducts_ <= firstProductIndex)
            {
                errorOnSearch_ = true;
                errorMessage_ = "No products found";
                return;
            }

            int amazonMaxPageNr = (searchIndex == "All") ? Globals.AMAZON_API_MAX_PAGES_INDEX_ALL : Globals.AMAZON_API_MAX_PAGES_INDEX_OTHER;

            int amazonFirstPage = firstProductIndex / Globals.AMAZON_API_MAX_PRODUCTS_PER_PAGE + 1;
            int amazonLastPage = lastProductIndex / Globals.AMAZON_API_MAX_PRODUCTS_PER_PAGE + 1;

            if (amazonLastPage > amazonMaxPageNr)
            {
                amazonLastPage = amazonMaxPageNr;
            }

            int amazonFirstIndex = firstProductIndex - ( amazonFirstPage - 1) * Globals.AMAZON_API_MAX_PRODUCTS_PER_PAGE;

            RetrieveProductsFromPages(amazonFirstPage, amazonLastPage, amazonFirstIndex);
        }

        public List<Product> Products()
        {
            if (errorOnSearch_) 
                return null;

            return foundProducts_;
        }

        public bool ErrorOnSearch()
        {
            return errorOnSearch_;
        }

        public string ErrorMessage()
        {
            return errorMessage_;
        }

        public string SearchIndex()
        {
            return searchIndex_;
        }

        public string SearchKeywords()
        {
            return searchKeywords_;
        }

        private void RetrieveProductsFromPages(int firstPage, int lastPage, int firstIndex )
        {
            errorOnSearch_ = false;
            errorMessage_ = "";

            // Batch requests in pairs of two. AMAZON Product Advertising API allows only two requests to be batched together [December 20th 2012]
            int currentIndex = 0;
            for (int pageNr = firstPage; pageNr <= lastPage; pageNr += 2 )
            {
                String itemPagesStr = (pageNr + 1 <= totalProducts_ / Globals.AMAZON_API_MAX_PRODUCTS_PER_PAGE) ? "&ItemSearch.1.ItemPage=" + pageNr + "&ItemSearch.2.ItemPage=" + (pageNr + 1) :
                                                                                                                  "&ItemPage=" + pageNr;
                String requestString = searchRequestHeader_ +
                                       "&ItemSearch.Shared.SearchIndex=" + searchIndex_ +
                                       itemPagesStr +
                                       "&ItemSearch.Shared.Keywords=" + searchKeywords_ +
                                       "&ItemSearch.Shared.Availability=Available" +
                                       "&ItemSearch.Shared.ResponseGroup=Small,Offers" +
                                       "&ItemSearch.Shared.Condition=All";

                String requestUrl = helper.Sign(requestString);
                XmlDocument doc = new XmlDocument();
                try
                {
                    WebRequest request = HttpWebRequest.Create(requestUrl);
                    WebResponse response = request.GetResponse();

                    doc.Load(response.GetResponseStream());
                }
                catch ( Exception e )
                {
                    errorOnSearch_ = true;
                    errorMessage_ = e.Message;
                    return;
                }

                // Check for error response
                XmlNodeList errorMessages = doc.GetElementsByTagName("Message");
                if (errorMessages != null && errorMessages.Count > 0)
                {
                    errorOnSearch_ = true;
                    errorMessage_ = errorMessages.Item(0).InnerText;
                    return;
                }
                
                foreach( XmlNode node in doc.GetElementsByTagName("Item") ) {
                    if (currentIndex >= firstIndex)
                    {
                        if (foundProducts_.Count == Globals.PRODUCTS_PER_PAGE)
                            break;

                        var product = new Product();
                        product.Title = node["ItemAttributes"]["Title"].InnerText;
                        product.DetailPageLink = node["DetailPageURL"].InnerText;
                        try
                        {
                            product.LowestNewPrice = (int.Parse(node["OfferSummary"]["LowestNewPrice"]["Amount"].InnerText, null) / 100.0);
                        }
                        catch
                        {
                            product.LowestNewPrice = null;
                        }
                        try
                        {
                            product.LowestUsedPrice = (int.Parse(node["OfferSummary"]["LowestUsedPrice"]["Amount"].InnerText, null) / 100.0);
                        }
                        catch
                        {
                            product.LowestUsedPrice = null;
                        }
                        foundProducts_.Add(product);
                    }
                    currentIndex++;
                }


            }
        }

        public int TotalPages()
        { 
            return totalProducts_ / Globals.PRODUCTS_PER_PAGE; 
        }

        private int FoundProductsCount(string searchIndex, string keywords )
        {
            String requestString = searchRequestHeader_ +
                             "&SearchIndex=" + searchIndex +
                             "&Availability=Available" +
                             "&ResponseGroup=Small" +
                             "&Keywords=" + keywords;

            String requestUrl = helper.Sign(requestString);

            WebRequest request = HttpWebRequest.Create(requestUrl);
            WebResponse response = request.GetResponse();

            XmlDocument doc = new XmlDocument();
            doc.Load(response.GetResponseStream());

            
            XmlNode itemsNode = doc.GetElementsByTagName("Items")[0];

            int totalProducts = 0;
            try
            {
                totalProducts = int.Parse(itemsNode["TotalResults"].InnerText, null);

                int maxPages = (searchIndex == "All") ? Globals.AMAZON_API_MAX_PAGES_INDEX_ALL : Globals.AMAZON_API_MAX_PAGES_INDEX_OTHER;
                if (totalProducts > maxPages * Globals.AMAZON_API_MAX_PRODUCTS_PER_PAGE)
                    totalProducts = maxPages * Globals.AMAZON_API_MAX_PRODUCTS_PER_PAGE;
            }
            catch
            {
                totalProducts = 0;
            }

            return totalProducts;
        }


        private List<Product> foundProducts_ = new List<Product>();

        private string searchRequestHeader_ = "Service=AWSECommerceService" +
                                             "&Version=2011-08-01" +
                                             "&AssociateTag=" + Globals.ASSOCIATE_TAG +
                                             "&Operation=ItemSearch";

        private int totalProducts_;
        private string searchIndex_;
        private string searchKeywords_;

        private bool errorOnSearch_;
        private string errorMessage_;

    }
}