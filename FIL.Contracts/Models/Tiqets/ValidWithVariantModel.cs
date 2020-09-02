using System.Collections.Generic;

namespace FIL.Contracts.Models.Tiqets
{
    public class ValidWithVariantModel
    {
        public long EventTicketDetailId { get; set; }
        public List<long> ValidWithEventTicketDetailId { get; set; }
    }
}