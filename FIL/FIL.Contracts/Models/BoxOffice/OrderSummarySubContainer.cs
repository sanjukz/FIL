using System.Collections.Generic;

namespace FIL.Contracts.Models.BoxOffice
{
    public class OrderSummarySubContainer
    {
        public Event Event { get; set; }
        public List<SubEventContainer> subEventContainer { get; set; }
    }
}