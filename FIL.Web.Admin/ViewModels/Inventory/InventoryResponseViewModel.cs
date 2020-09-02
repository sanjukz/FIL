using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.Inventory
{
    public class InventoryResponseViewModel
    {
        public bool Success { get; set; }
        public Guid EventAltId { get; set; }
        public List<TicketCategory> TicketCategories { get; set; }
        public List<EventTicketDetail> EventTicketDetails { get; set; }
        public List<EventTicketAttribute> EventTicketAttributes { get; set; }
        public PlaceTicketRedemptionDetail PlaceTicketRedemptionDetails { get; set; }
        public List<PlaceCustomerDocumentTypeMapping> PlaceCustomerDocumentTypeMappings { get; set; }
        public List<EventDeliveryTypeDetail> EventDeliveryTypeDetails { get; set; }
        public List<FIL.Contracts.DataModels.EventHostMapping> EventHosts { get; set; }
    }
}
