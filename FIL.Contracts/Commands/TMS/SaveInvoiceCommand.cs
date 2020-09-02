using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.TMS
{
    public class SaveInvoiceCommand : ICommandWithResult<SaveInvoiceCommandResult>
    {
        public DateTime InvoiceDueDate { get; set; }
        public int CompanyDetailId { get; set; }
        public long BankDetailId { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int CurrencyId { get; set; }
        public List<CorporateOrderRequestDetail> CorporateOrderRequestDetails { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateOrderRequestDetail
    {
        public long CorporateOrderRequestId { get; set; }
        public int Quantity { get; set; }
        public decimal localPrice { get; set; }
        public decimal ConvenienceCharge { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal ValueAddedTax { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class SaveInvoiceCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}