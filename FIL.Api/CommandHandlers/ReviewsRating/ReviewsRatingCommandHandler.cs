using FIL.Api.Repositories;
using FIL.Contracts.Commands.ReviewsRating;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ReviewsRating
{
    public class ReviewsRatingCommandHandler : BaseCommandHandler<ReviewsRatingCommand>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        public ReviewsRatingCommandHandler(IRatingRepository ratingRepository,
            IMediator mediator,
            IUserRepository userRepository,
            IEventRepository eventRepository)
            : base(mediator)
        {
            _ratingRepository = ratingRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        protected override async Task Handle(ReviewsRatingCommand command)
        {
            var user = _userRepository.GetByAltId(command.UserAltId);
            var eventDetails = _eventRepository.GetByAltId(command.EventAltId);
            var ratingReviews = new Rating
            {
                UserId = user.Id,
                EventId = eventDetails.Id,
                Points = command.Points,
                Comment = command.Comment,
                CreatedUtc = DateTime.UtcNow,
                CreatedBy = user.AltId,
                IsEnabled = command.IsEnabled,
                UpdatedBy = user.AltId,
                UpdatedUtc = DateTime.UtcNow
            };
            _ratingRepository.Save(ratingReviews);
            return;
        }
    }
}