using FIL.Api.Repositories;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Countries
{
    public class SaveCountryCommandHandler : BaseCommandHandler<SaveCountryCommand>
    {
        private readonly ICountryRepository _countryRepository;

        public SaveCountryCommandHandler(ICountryRepository countryRepository, IMediator mediator)
            : base(mediator)
        {
            _countryRepository = countryRepository;
        }

        protected override async Task Handle(SaveCountryCommand command)
        {
            var country = new Country
            {
                AltId = Guid.NewGuid(),
                Name = command.Name,
                IsoAlphaTwoCode = command.IsoAlphaTwoCode,
                IsoAlphaThreeCode = command.IsoAlphaThreeCode,
                ModifiedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                IsEnabled = true
            };

            _countryRepository.Save(country);
        }
    }
}