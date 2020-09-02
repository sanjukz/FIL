using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.ReviewsAndRating;
using System;

namespace FIL.Contracts.Queries.ReviewsAndRating
{
    public class ReviewsAndRatingQuery : IQuery<ReviewsAndRatingQueryResult>
    {
        public Guid UserAltId { get; set; }
        public Guid eventAltId { get; set; }
    }
}