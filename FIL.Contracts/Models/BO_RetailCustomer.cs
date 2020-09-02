using System;

namespace FIL.Contracts.Models
{
    public class BO_RetailCustomer
    {
        public long Id { get; set; }
        public long Trans_Id { get; set; }
        public long Retailer_Id { get; set; }
        public string Cust_FirstName { get; set; }
        public string Cust_LastName { get; set; }
        public string Cust_Address { get; set; }
        public string Cust_City { get; set; }
        public string Cust_PinCode { get; set; }
        public string Cust_State { get; set; }
        public int Cust_PaymentMode { get; set; }
        public string Cust_MobileNumber { get; set; }
        public string Cust_Event { get; set; }
        public long Cust_EventID { get; set; }
        public string OrderNumber { get; set; }
        public string Cust_IdType { get; set; }
        public string Cust_IdTypeNumber { get; set; }
        public DateTime DateandTime { get; set; }
        public string TicketCategory { get; set; }
        public int NoOfTic { get; set; }
        public string TheaterName { get; set; }
        public string MovieName { get; set; }
        public string PricePerTic { get; set; }
        public string ShowDateTime { get; set; }
        public string ThCity { get; set; }
        public decimal TotalCharge { get; set; }
        public decimal ConvenienceCharge { get; set; }
        public decimal ServiceTax { get; set; }
        public decimal DealerAccessCharge { get; set; }
        public decimal TotalAmountPayable { get; set; }
        public short UpdateDate { get; set; }
        public string Cust_Country { get; set; }
        public bool IsVerified { get; set; }
        public string Cust_Address2 { get; set; }
        public string Cust_Address3 { get; set; }
        public string Cust_Dob { get; set; }
        public string CompanyName { get; set; }
        public string PaymentType { get; set; }
        public string Discount { get; set; }
        public string Discount_Comment { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}