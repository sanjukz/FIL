using System;

namespace FIL.Contracts.Models.Integrations.Kidzania
{
    public class BuyOption
    {
        public long ParkId { get; set; }
        public string PayType { get; set; }
        public string PayConfNo { get; set; }
        public string Remarks { get; set; }
        public string MobileNo { get; set; }
        public DateTime VisitDate { get; set; }
        public long ShiftId { get; set; }
        public string OrderId { get; set; }
        public string OrderRef { get; set; }
        public string MTR { get; set; }
        public string TransactionId { get; set; }
        public string RRN { get; set; }
        public string PayAuthId { get; set; }
    }
}