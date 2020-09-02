using FIL.Contracts.Enums;

namespace FIL.Contracts.Models
{
    public class TicketCategoryDetail
    {
        public int Id { get; set; }
        public int TicketCategoryId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public Channels Channel { get; set; }
        public bool IsEnabled { get; set; }
    }
}