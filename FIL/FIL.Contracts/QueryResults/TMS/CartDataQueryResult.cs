using System.Collections.Generic;

namespace FIL.Contracts.QueryResult.TMS
{
    public class CartDataQueryResult
    {
        public List<CartDataModel> cartDataList { get; set; }
    }

    public class CartDataModel
    {
        public string Name { get; set; }
        public string startDateTime { get; set; }
        public string venueName { get; set; }
        public string city { get; set; }
        public string ticketCategoryName { get; set; }
        public decimal price { get; set; }
        public string currencyCode { get; set; }
        public string deliveryType { get; set; }
    }
}