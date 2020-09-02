using FIL.Api.Repositories;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Venues
{
    public class SaveVenueCommandHandler : BaseCommandHandler<SaveVenueCommand>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IVenueRepository _venueRepository;

        public SaveVenueCommandHandler(ICityRepository cityRepository, IVenueRepository venueRepository, IMediator mediator)
            : base(mediator)
        {
            _cityRepository = cityRepository;
            _venueRepository = venueRepository;
        }

        protected override async Task Handle(SaveVenueCommand command)
        {
            var city = _cityRepository.GetByAltId(command.CityAltId);
            if (city != null)
            {
                var venue = new Venue
                {
                    AltId = Guid.NewGuid(),
                    Name = command.Name,
                    AddressLineOne = command.AddressLineOne,
                    AddressLineTwo = command.AddressLineTwo,
                    CityId = city.Id,
                    Latitude = command.Latitude,
                    Longitude = command.Longitude,
                    HasImages = command.HasImages,
                    Prefix = command.Prefix,
                    ModifiedBy = command.ModifiedBy,
                    IsEnabled = true
                };

                _venueRepository.Save(venue);
            }
        }
    }
}