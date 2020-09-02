using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.Report
{
    public class ExternalTranscationReportContainer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public Guid TransactionAltId { get; set; }
        public Guid UserAltId { get; set; }
        public string UserName { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentType { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public List<TicketCategoryContainer> ticketCategoryContainer { get; set; }
        public string ExportStatus { get; set; }
    }
}