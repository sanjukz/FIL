using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Boxoffice.TicketCategories
{
    public class TicketCategoryQueryResult
    {
        public BoxofficeUserAdditionalDetail boxofficeUserAdditionalDetail { get; set; }
        public IEnumerable<ZsuiteUserFeeDetail> zsuiteUserFeeDetail { get; set; }
        public IEnumerable<ZsuitePaymentOption> zsuitePaymentOptions { get; set; }
        public EventDetail eventDetail { get; set; }
        public Venue venue { get; set; }
        public City city { get; set; }
        public List<Models.BoxOffice.TicketCategoryContainer> ticketCategoryContainer { get; set; }
    }
}