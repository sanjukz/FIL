using FIL.Contracts.Models;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.PlaceInventory
{
    public class PlaceInventoryCommand : Contracts.Interfaces.Commands.ICommandWithResult<PlaceInventoryCommandCommandResult>
    {
        public List<TicketCategoriesViewModel> ticketCategoriesViewModels { get; set; }
        public Guid PlaceAltId { get; set; }
        public List<Guid> EventDetailAltId { get; set; }
        public string TermsAndCondition { get; set; }
        public List<int> DeliverType { get; set; }
        public List<int> CustomerIdTypes { get; set; }
        public bool IsCustomerIdRequired { get; set; }
        public string RedemptionInstructions { get; set; }
        public List<int> CustomerInformation { get; set; }
        public string RedemptionAddress { get; set; }
        public string RedemptionCity { get; set; }
        public string RedemptionState { get; set; }
        public string RedemptionCountry { get; set; }
        public bool IsEdit { get; set; }
        public int RefundPolicy { get; set; }
        public string RedemptionZipcode { get; set; }
        public DateTime RedemptionDateTime { get; set; }
        public bool IsUpdatedDeliverType { get; set; }
        public bool IsUpdatedCustomerIdTypes { get; set; }
        public bool IsUpdatedTermsAndCondition { get; set; }
        public Guid ModifiedBy { get; set; }
        public List<FeeTypes> FeeTypes { get; set; }
    }

    public class TicketCategoriesViewModel
    {
        public string categoryName { get; set; }
        public int EventTicketDetailId { get; set; }
        public int TicketCategoryId { get; set; }
        public int Quantity { get; set; }
        public string TicketCategoryDescription { get; set; }
        public string TicketCategoryNote { get; set; }
        public int CurrencyId { get; set; }
        public float PricePerTicket { get; set; }
        public bool IsRollingTicketValidityType { get; set; }
        public DateTime TicketValidityFixDate { get; set; }
        public string Days { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public bool IsEventTicketAttributeUpdated { get; set; }
        public long TicketCategoryTypeId { get; set; }
        public long TicketSubCategoryTypeId { get; set; }
    }

    public class PlaceInventoryCommandCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public List<TicketCategory> TicketCategories { get; set; }
        public List<EventTicketDetail> EventTicketDetails { get; set; }
        public List<EventTicketAttribute> EventTicketAttributes { get; set; }
        public PlaceTicketRedemptionDetail PlaceTicketRedemptionDetails { get; set; }
        public List<PlaceCustomerDocumentTypeMapping> PlaceCustomerDocumentTypeMappings { get; set; }
        public List<EventDeliveryTypeDetail> EventDeliveryTypeDetails { get; set; }
    }

    public class FeeTypes
    {
        public int FeeTypeId { get; set; }
        public int ValueTypeId { get; set; }
        public decimal Value { get; set; }
        public string FeeType { get; set; }
        public string CategoryName { get; set; }
        public long EventTicketAttributeId { get; set; }
    }
}