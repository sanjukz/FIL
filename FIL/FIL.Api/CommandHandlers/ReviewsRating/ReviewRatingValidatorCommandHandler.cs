using FIL.Api.Repositories;
using FIL.Contracts.Commands.ReviewsRating;
using FIL.Contracts.DataModels;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ReviewsRating
{
    public class ReviewRatingValidatorCommandHandler : BaseCommandHandler<ActiveReviewCommand>
    {
        private readonly IRatingRepository _ratingRepository;

        public ReviewRatingValidatorCommandHandler(IRatingRepository ratingRepository, IMediator mediator) : base(mediator)
        {
            _ratingRepository = ratingRepository;
        }

        protected override async Task Handle(ActiveReviewCommand command)
        {
            var ratingDetails = _ratingRepository.GetByAltId(command.AltId);
            var ratingReviews = new Rating
            {
                Id = ratingDetails.Id,
                AltId = ratingDetails.AltId,
                UserId = ratingDetails.UserId,
                EventId = ratingDetails.EventId,
                Points = ratingDetails.Points,
                Comment = ratingDetails.Comment,
                CreatedUtc = ratingDetails.CreatedUtc,
                CreatedBy = ratingDetails.CreatedBy,
                IsEnabled = true,
                UpdatedBy = ratingDetails.UpdatedBy,
                UpdatedUtc = ratingDetails.UpdatedUtc
            };
            _ratingRepository.Save(ratingReviews);
            return;
        }
    }
}