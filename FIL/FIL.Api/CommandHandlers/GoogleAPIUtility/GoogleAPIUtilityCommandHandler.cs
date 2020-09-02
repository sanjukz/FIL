using FIL.Api.CommandHandlers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.GoogleAPIUtility;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.PlaceCalendar
{
    public class GoogleAPIUtilityCommandHandler : BaseCommandHandlerWithResult<GoogleAPIUtilityCommand, GoogleAPIUtilityCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IVenueRepository _venueRepository;

        public GoogleAPIUtilityCommandHandler(IMediator mediator,
            IVenueRepository venueRepository) : base(mediator)
        {
            _mediator = mediator;
            _venueRepository = venueRepository;
        }

        protected override async Task<ICommandResult> Handle(GoogleAPIUtilityCommand command)
        {
            try
            {
                var venue = _venueRepository.Get(command.VenueId);
                venue.Latitude = command.Latitude;
                venue.Longitude = command.Longitude;
                _venueRepository.Save(venue);
                return new GoogleAPIUtilityCommandResult
                {
                    Success = true,
                };
            }
            catch (Exception e)
            {
                return new GoogleAPIUtilityCommandResult
                {
                    Success = false
                };
            }
        }
    }
}