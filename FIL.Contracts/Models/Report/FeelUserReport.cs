using System;

namespace FIL.Contracts.Models.Report
{
    public class FeelUserReport
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IPAddress { get; set; }
        public string IPCity { get; set; }
        public string IPCountry { get; set; }
        public string IPState { get; set; }
        public string SignUpMethod { get; set; }
        public bool IsTransacted { get; set; }
        public string IsOptIn { get; set; }
        public string IsCreator { get; set; }
    }
}