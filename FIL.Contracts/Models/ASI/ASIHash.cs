using System;

namespace FIL.Contracts.Models.ASI
{
    public class ASIHash
    {
        public string Code { get; set; }
        public bool IsOptional { get; set; }
        public DateTime Date { get; set; }
        public int Age { get; set; }
        public string VisitorId { get; set; }
        public string Nationality { get; set; }
        public decimal Amount { get; set; }
        public String IdentityType { get; set; }
        public string IdentityNumber { get; set; }
    }
}