using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.EventWizard
{
    public class EventTicketAttributeFormDataViewModel
    {
        [Required]
        public DateTime SalesStartDateTime { get; set; }
        [Required]
        public DateTime SalesEndDatetime { get; set; }
        [Required]
        public TicketType TicketTypeId { get; set; }
        [Required]
        public Channels ChannelId { get; set; }
        [Required]
        public int CurrencyId { get; set; }     
        public short? SharedInventoryGroupId { get; set; }
        [Required]
        public short AvailableTicketForSale { get; set; }
        [Required]
        public short RemainingTicketForSale { get; set; }
        [Required]
        public string TicketCategoryDescription { get; set; }
        [Required]
        public string ViewFromStand { get; set; }        
        public bool? IsSeatSelection { get; set; }
        [Required]
        public decimal Price { get; set; }        
        public bool? IsInternationalCardAllowed { get; set; }        
        public bool? IsEMIApplicable { get; set; }
    }
}
