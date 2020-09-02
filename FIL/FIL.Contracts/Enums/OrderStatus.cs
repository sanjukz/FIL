using FIL.Contracts.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum OrderStatus
    {
        None = 0,

        [Display(Name = "Approve")]
        Approve,

        [Display(Name = "Pending")]
        Pending,

        [Display(Name = "Reject")]
        Reject,

        [Display(Name = "Invoice Pending")]
        InvoicePending,

        [Display(Name = "Invoice Generated")]
        InvoiceGenerated,

        [Display(Name = "Payment Received")]
        PaymentReceived
    }
}