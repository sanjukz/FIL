using System;

namespace FIL.Contracts.Models
{
    public class DynamicStadiumTicketCategoriesDetails
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public int DynamicStadiumCoordinateId { get; set; }
        public int TicketCategoryId { get; set; }
        public int FillingFastPercentage { get; set; }
    }
}