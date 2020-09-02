using System.Collections.Generic;

namespace FIL.Contracts.Models.Tiqets
{
    public class PriceComponentsEur
    {
        public double distributor_commission_excl_vat { get; set; }
        public double total_retail_price_incl_vat { get; set; }
        public double sale_ticket_value_incl_vat { get; set; }
        public double booking_fee_incl_vat { get; set; }
    }

    public class Variant
    {
        public IList<int> valid_with_variant_ids { get; set; }
        public int max_tickets { get; set; }
        public string description { get; set; }
        public bool dynamic_variant_pricing { get; set; }
        public IList<object> requires_visitors_details { get; set; }
        public string label { get; set; }
        public PriceComponentsEur price_components_eur { get; set; }
        public int id { get; set; }
    }

    public class VariantResponseModel
    {
        public bool success { get; set; }
        public IList<Variant> variants { get; set; }
        public int? max_tickets_per_order { get; set; }
    }
}