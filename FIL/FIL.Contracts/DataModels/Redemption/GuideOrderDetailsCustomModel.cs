using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Redemption
{
    public class GuideOrderDetailsCustomModel
    {
        public long TransactionId { get; set; }
        public string PlaceName { get; set; }
        public string PlaceCity { get; set; }
        public string PlaceState { get; set; }
        public string PlaceCountry { get; set; }
        public DateTime VisitDate { get; set; }
        public string TicketCategory { get; set; }
        public int TicketCategoryId { get; set; }
        public int EventTicketAttribueId { get; set; }
        public string OrderStatus { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public DateTime OrderApprovedDate { get; set; }
        public DateTime OrderCompletedDate { get; set; }
        public Guid OrderApprovedBy { get; set; }
        public string Currency { get; set; }
        public decimal TicketPrice { get; set; }
        public string GuideFirstName { get; set; }
        public string GuideLastName { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}