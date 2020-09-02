using FIL.Api.Repositories;
using FIL.Contracts.Commands.MasterVenueLayoutSections;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.MasterVenueLayoutSections
{
    public class SaveVenueCommandHandler : BaseCommandHandlerWithResult<SaveVenueCommand, SaveSaveVenueCommandResult>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IZipcodeRepository _zipcodeRepository;
        private readonly IMasterVenueLayoutRepository _masterVenueLayoutRepository;

        public SaveVenueCommandHandler(IMasterVenueLayoutRepository masterVenueLayoutRepository, ICountryRepository countryRepository, IStateRepository stateRepository,
       IEventGalleryImageRepository eventGalleryImageRepository,
       IZipcodeRepository zipcodeRepository,
           ICityRepository cityRepository, IVenueRepository venueRepository, IMediator mediator) : base(mediator)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _venueRepository = venueRepository;
            _zipcodeRepository = zipcodeRepository;
            _masterVenueLayoutRepository = masterVenueLayoutRepository;
        }

        protected override async Task<ICommandResult> Handle(SaveVenueCommand command)
        {
            Country savedCountry = new Country();
            var countryinfo = new Country();
            if (command.Country == "United Kingdom" || command.Country == "U.K.")
            {
                countryinfo = _countryRepository.Get(231);
            }
            else if (command.Country == "United States" || command.Country == "U.S.A.")
            {
                countryinfo = _countryRepository.Get(230);
            }
            else
            {
                countryinfo = _countryRepository.GetByName(command.Country);
            }
            if (countryinfo == null)
            {
                var country = new Country
                {
                    AltId = Guid.NewGuid(),
                    Name = command.Country,
                    IsoAlphaThreeCode = "NA",
                    IsoAlphaTwoCode = "NA",
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true
                };
                savedCountry = _countryRepository.Save(country);
            }
            else
            {
                savedCountry.AltId = countryinfo.AltId;
                savedCountry.Id = countryinfo.Id;
            }
            //--------------------------------------------------------------------
            var stateinfo = _stateRepository.GetByNameAndCountryId(command.State, savedCountry.Id);
            State state = new State();

            if (stateinfo == null)
            {
                state = _stateRepository.Save(new
                     State()
                {
                    AltId = Guid.NewGuid(),
                    CountryId = savedCountry.Id,
                    IsEnabled = true,
                    Abbreviation = "NA",
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.Now,
                    Name = command.State,
                    UpdatedBy = command.ModifiedBy,
                    ModifiedBy = command.ModifiedBy,
                    UpdatedUtc = DateTime.Now
                });
            }
            else
            {
                state.Id = stateinfo.Id;
                state.AltId = stateinfo.AltId;
            }
            //--------------------------------------------------------------------
            var cityinfo = _cityRepository.GetByNameAndStateId(command.City, state.Id);
            City city = new City();
            if (cityinfo == null)
            {
                city = _cityRepository.Save(new
                    City()
                {
                    AltId = Guid.NewGuid(),
                    Name = command.City,
                    StateId = state.Id,
                    IsEnabled = true,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.Now,
                    UpdatedBy = command.ModifiedBy,
                    ModifiedBy = command.ModifiedBy,
                    UpdatedUtc = DateTime.Now
                });
            }
            else
            {
                city.Id = cityinfo.Id;
                city.AltId = cityinfo.AltId;
            }
            //--------------------------------------------------------------------
            var venueinfo = _venueRepository.GetByNameAndCityId(command.VenueName, city.Id);
            Venue venue = new Venue();
            if (venueinfo == null)
            {
                venue = _venueRepository.Save(new
                       Venue()
                {
                    AltId = Guid.NewGuid(),
                    Name = command.VenueName,
                    CityId = city.Id,
                    AddressLineOne = command.AddressLineOne,
                    AddressLineTwo = command.AddressLineTwo,
                    Latitude = command.Latitude,
                    Longitude = command.Longitude,
                    IsEnabled = true,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.Now,
                    UpdatedBy = command.ModifiedBy,
                    ModifiedBy = command.ModifiedBy,
                    UpdatedUtc = DateTime.Now
                });
            }
            else
            {
                venue.Id = venueinfo.Id;
                venue.AltId = venueinfo.AltId;
            }
            //-----------------------------------------------------
            var mastervenuelayoutinfo = _masterVenueLayoutRepository.GetByName(command.VenueName);
            MasterVenueLayout masterVenueLayout = new MasterVenueLayout();
            if (mastervenuelayoutinfo == null)
            {
                masterVenueLayout = _masterVenueLayoutRepository.Save(new MasterVenueLayout()
                {
                    AltId = Guid.NewGuid(),
                    LayoutName = command.VenueName,
                    VenueId = venue.Id,
                    IsEnabled = true,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.Now,
                    UpdatedBy = command.ModifiedBy,
                    ModifiedBy = command.ModifiedBy,
                    UpdatedUtc = DateTime.Now
                });
                return new SaveSaveVenueCommandResult
                {
                    Success = true,
                    LayoutAltId = masterVenueLayout.AltId.ToString(),
                    IsExisting = false
                };
            }
            return new SaveSaveVenueCommandResult
            {
                Success = false,
                LayoutAltId = masterVenueLayout.AltId.ToString(),
                IsExisting = true
            };
        }
    }
}