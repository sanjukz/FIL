using FIL.Api.Repositories;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Zipcodes
{
    public class SaveZipcodeCommandHandler : BaseCommandHandler<SaveZipcodeCommand>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IZipcodeRepository _zipcodeRepository;

        public SaveZipcodeCommandHandler(ICityRepository cityRepository, IZipcodeRepository zipcodeRepository, IMediator mediator)
            : base(mediator)
        {
            _cityRepository = cityRepository;
            _zipcodeRepository = zipcodeRepository;
        }

        protected override async Task Handle(SaveZipcodeCommand command)
        {
            var city = _cityRepository.GetByAltId(command.CityAltId);
            if (city != null)
            {
                var zipcode = new Zipcode
                {
                    AltId = Guid.NewGuid(),
                    Postalcode = command.Zipcode,
                    Region = command.Region,
                    CityId = city.Id,
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true
                };

                _zipcodeRepository.Save(zipcode);
            }
        }
    }
}