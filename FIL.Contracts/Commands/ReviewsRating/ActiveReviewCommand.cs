using System;

namespace FIL.Contracts.Commands.ReviewsRating
{
    public class ActiveReviewCommand : BaseCommand
    {
        public Guid AltId { get; set; }
    }
}