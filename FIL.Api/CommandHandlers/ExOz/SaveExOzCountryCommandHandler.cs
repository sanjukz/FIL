using FIL.Api.Repositories;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ExOz
{
    public class SaveExOzCountryCommandHandler : BaseCommandHandlerWithResult<SaveExOzCountryCommand, SaveExOzCountryCommandResult>
    {
        private readonly IExOzCountryRepository _exOzCountryRepository;
        private readonly ICountryRepository _countryRepository;

        public SaveExOzCountryCommandHandler(IExOzCountryRepository exOzCountryRepository, ICountryRepository countryRepository, IMediator mediator)
            : base(mediator)
        {
            _exOzCountryRepository = exOzCountryRepository;
            _countryRepository = countryRepository;
        }

        protected override Task<ICommandResult> Handle(SaveExOzCountryCommand command)
        {
            _exOzCountryRepository.DisableAllExOzCountries();

            SaveExOzCountryCommandResult exOzCountryResultList = new SaveExOzCountryCommandResult();
            exOzCountryResultList.CountryList = new List<ExOzCountry>();

            var exOzCountries = _exOzCountryRepository.GetAll(null);
            var kzCountries = _countryRepository.GetByNames(command.Names);

            //Save ExOzCountries
            foreach (var item in command.Names)
            {
                var existingExOzCountry = exOzCountries.Where(w => w.Name == item).FirstOrDefault();
                var existingKzCountry = kzCountries.Where(w => w.Name == item).FirstOrDefault();
                if (existingKzCountry == null)
                {
                    Country newKzCountry = new Country
                    {
                        Name = item,
                        IsEnabled = true,
                    };
                    Country countrySaved = _countryRepository.Save(newKzCountry);
                }

                ExOzCountry exOzCountryResult = new ExOzCountry();
                if (existingExOzCountry == null)
                {
                    ExOzCountry newExOzCountry = new ExOzCountry
                    {
                        Name = item,
                        CountryMapId = existingKzCountry.Id,
                        ModifiedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        IsEnabled = true,
                    };
                    exOzCountryResult = _exOzCountryRepository.Save(newExOzCountry);
                }
                exOzCountryResultList.CountryList.Add(exOzCountryResult);
            }
            return Task.FromResult<ICommandResult>(exOzCountryResultList);
        }
    }
}