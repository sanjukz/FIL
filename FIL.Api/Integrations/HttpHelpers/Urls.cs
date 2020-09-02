namespace FIL.Api.Integrations.HttpHelpers
{
    internal static class Urls
    {
        public static string BaseUrl { get; set; }

        public static string Token
        {
            get { return BaseUrl + "/oauth2/accesstoken"; }
        }

        public static string Performance
        {
            get { return BaseUrl + "/performances"; }
        }

        public static string Availability
        {
            get { return BaseUrl + "/performances/{0}/availabilities?channel={1}&sellerCode={2}"; }
        }

        public static string Price
        {
            get { return BaseUrl + "/performances/{0}/prices?channel={1}&sellerCode={2}"; }
        }

        public static string Section
        {
            get { return BaseUrl + "/performances/{0}/sections/{1}?channel={2}&sellerCode={3}"; }
        }

        public static string CreateBasket
        {
            get { return BaseUrl + "/baskets"; }
        }

        public static string LookUpBasket
        {
            get { return BaseUrl + "/baskets/{0}"; }
        }

        public static string RmoveBasket
        {
            get { return BaseUrl + "/baskets/{0}"; }
        }

        public static string AddOffer
        {
            get { return BaseUrl + "/baskets/{0}/offers"; }
        }

        public static string LookUpOffer
        {
            get { return BaseUrl + "/baskets/{0}/offers/{1}"; }
        }

        public static string PurchaseBasket
        {
            get { return BaseUrl + "/baskets/{0}/purchase"; }
        }

        public static string AddFee
        {
            get { return BaseUrl + "/baskets/{0}/fees"; }
        }

        public static string UpdateFeeCode
        {
            get { return BaseUrl + "/baskets/{0}/fees/{1}/{2}"; }
        }

        public static string RemoveFeeCode
        {
            get { return BaseUrl + "/baskets/{0}/fees/{1}/{2}"; }
        }

        public static string CreateCustomer
        {
            get { return BaseUrl + "/customers?sellerCode={0}"; }
        }

        public static string LookUpCustomer
        {
            get { return BaseUrl + "/customers/{0}?sellerCode={1}"; }
        }

        public static string UpdateCustomer
        {
            get { return BaseUrl + "/customers/{0}?sellerCode={1}"; }
        }

        public static string LookUpOrder
        {
            get { return BaseUrl + "/orders/{0}?sellerCode={1}"; }
        }

        public static string ReverseOrder
        {
            get { return BaseUrl + "/orders/{0}/reverse"; }
        }

        public static class InfiniteAnalytics
        {
            public static string Session
            {
                get
                {
                    return BaseUrl + "/sessions/init?ecompany=feelaplace.com";
                }
            }

            public static string Recommendation
            {
                get
                {
                    return BaseUrl + "/recommendations/any?ecompany=feelaplace.com&session_id={0}&client_type={1}&site_page_type={2}&site_product_id={3}&count={4}";
                }
            }
        }
    }
}