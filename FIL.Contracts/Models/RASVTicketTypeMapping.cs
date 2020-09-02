using FIL.Contracts.Enums;

namespace FIL.Contracts.Models
{
    public class RASVTicketTypeMapping
    {
        public int Id { get; set; }
        public long EventDetailId { get; set; }
        public RASVTicketType RASVTicketTypeId { get; set; }
    }
}