using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class OrderConfirmationSubContainer
    {
        public Event Event { get; set; }
        public List<SubEventContainer> subEventContainer { get; set; }
    }
}