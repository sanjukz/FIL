namespace FIL.Contracts.Models.Tiqets
{
    public class AdditionalInformation
    {
        public string must_know { get; set; }
        public string good_to_know { get; set; }
        public string pre_purchase { get; set; }
        public string usage { get; set; }
        public string excluded { get; set; }
        public string included { get; set; }
        public string post_purchase { get; set; }
    }

    public class CheckoutResponseModel
    {
        public bool success { get; set; }
        public AdditionalInformation additional_information { get; set; }
        public bool has_timeslots { get; set; }
        public bool has_dynamic_pricing { get; set; }
    }
}