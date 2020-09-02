using System.Collections.Generic;

namespace FIL.Contracts.Models.Tiqets
{
    public class Variants
    {
        public int variant_id { get; set; }
        public int count { get; set; }
    }

    public class CustomerDetails
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class CreateOrderRequestModel
    {
        public int product_id { get; set; }
        public string day { get; set; }
        public string timeslot { get; set; }
        public List<Variants> variants { get; set; }
        public CustomerDetails customer_details { get; set; }
    }
}