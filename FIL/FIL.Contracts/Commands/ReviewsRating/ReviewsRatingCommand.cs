using System;

namespace FIL.Contracts.Commands.ReviewsRating
{
    public class ReviewsRatingCommand : BaseCommand
    {
        public Guid UserAltId { get; set; }
        public Guid EventAltId { get; set; }
        public short Points { get; set; }
        public string Comment { get; set; }
        public bool IsEnabled { get; set; }
    }
}