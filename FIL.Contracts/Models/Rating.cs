using System;

namespace FIL.Contracts.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public long UserId { get; set; }
        public long EventId { get; set; }
        public short Points { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}