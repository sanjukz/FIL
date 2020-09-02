using FIL.Api.Repositories;
using FIL.Contracts.Commands.Location;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Location
{
    public class LocationCommandHandler : BaseCommandHandlerWithResult<LocationCommand, LocationCommandResult>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventGalleryImageRepository _eventGalleryImageRepository;
        private readonly IZipcodeRepository _zipcodeRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;

        public LocationCommandHandler(ICountryRepository countryRepository, IStateRepository stateRepository,
        IEventGalleryImageRepository eventGalleryImageRepository,
        IZipcodeRepository zipcodeRepository,
            IEventAttributeRepository eventAttributeRepository,
            ICityRepository cityRepository, IVenueRepository venueRepository, IEventDetailRepository eventDetailRepository, IMediator mediator) : base(mediator)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _venueRepository = venueRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventGalleryImageRepository = eventGalleryImageRepository;
            _zipcodeRepository = zipcodeRepository;
            _eventAttributeRepository = eventAttributeRepository;
        }

        protected override async Task<ICommandResult> Handle(LocationCommand command)
        {
            var countryinfo = new Country();
            if (command.Country == "United Kingdom" || command.Country == "U.K." || command.Country.ToLower() == "uk")
            {
                countryinfo = _countryRepository.Get(230);
            }
            else if (command.Country == "United States" || command.Country == "U.S.A." || command.Country.ToLower() == "u.s.a")
            {
                countryinfo = _countryRepository.Get(231);
            }
            else if (command.Country == "United Arab Emirates" || command.Country == "U.A.E." || command.Country.ToLower() == "u.a.e")
            {
                countryinfo = _countryRepository.Get(229);
            }
            else
            {
                countryinfo = _countryRepository.GetByName(command.Country);
            }
            Country savedCountry = new Country();
            if (countryinfo == null)
            {
                var country = new Country
                {
                    AltId = Guid.NewGuid(),
                    Name = command.Country,
                    CountryName = command.Country,
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
            //----------------------------------------------------------------------
            var zipCode = _zipcodeRepository.GetAllByCityId(city.Id).FirstOrDefault();
            if (zipCode != null && zipCode.Postalcode != command.Zip)
            {
                zipCode.Postalcode = command.Zip;
                _zipcodeRepository.Save(zipCode);
            }
            if (zipCode == null)
            {
                Zipcode zipcode = new Zipcode();
                zipcode.AltId = Guid.NewGuid();
                zipcode.Postalcode = command.Zip;
                zipcode.CityId = city.Id;
                zipcode.IsEnabled = true;
                zipcode.CreatedUtc = DateTime.UtcNow;
                zipcode.CreatedBy = Guid.NewGuid();
                _zipcodeRepository.Save(zipcode);
            }
            //--------------------------------------------------------------------
            var venueinfo = _venueRepository.GetAll().Where(p => p.Name.Contains(command.Address1)).LastOrDefault();
            Venue venue = new Venue();

            if (venueinfo == null || command.EventId > 0)
            {
                venue = _venueRepository.Save(new
                    Venue()
                {
                    AltId = Guid.NewGuid(),
                    Name = command.PlaceName,
                    CityId = city.Id,
                    AddressLineOne = command.Address1,
                    AddressLineTwo = command.Address2,
                    Latitude = command.Lat,
                    Longitude = command.Long,
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
            //--------------------------------------------------------------------

            long eventDetailId = 0;
            if (!command.IsEdit)
            {
                var eventDetailData = _eventDetailRepository.GetSubeventByEventId((int)command.EventId).FirstOrDefault();
                var allEventDetails = _eventDetailRepository.GetSubeventByEventId((int)command.EventId);
                try
                {
                    foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in allEventDetails)  // If creating new place which already exists then move old place to different eventId and create new eventdetail
                    {
                        currentEventDetail.EventId = 2384;
                        currentEventDetail.IsEnabled = false;
                        _eventDetailRepository.Save(currentEventDetail);
                    }
                }
                catch (Exception e)
                {
                }

                EventDetail eventDetail = new EventDetail();
                eventDetail.Id = command.EventDetailId;
                eventDetail.AltId = Guid.NewGuid();
                eventDetail.EventId = command.EventId;
                eventDetail.Description = command.Description;
                eventDetail.IsEnabled = true;
                eventDetail.Name = ((command.Title != null && command.Title != "") ? command.Title : "place");
                eventDetail.VenueId = venue.Id;
                eventDetail.CreatedBy = command.ModifiedBy;
                eventDetail.CreatedUtc = DateTime.Now;
                eventDetail.UpdatedBy = command.ModifiedBy;
                eventDetail.ModifiedBy = command.ModifiedBy;
                eventDetail.UpdatedUtc = DateTime.Now;
                eventDetail.StartDateTime = DateTime.Now;
                eventDetail.EndDateTime = DateTime.UtcNow.AddYears(10);
                eventDetail.GroupId = 1;
                var eventdetailsaved = _eventDetailRepository.Save(eventDetail);
                eventDetailId = eventdetailsaved.Id;
            }
            else
            {
                var eventdetailsaved = _eventDetailRepository.GetSubeventByEventId((int)command.EventId).ToList();
                if (!eventdetailsaved.Any())
                {
                    try
                    {
                        foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in eventdetailsaved)  // If creating new place which already exists then move old place to different eventId and create new eventdetail
                        {
                            currentEventDetail.EventId = 2384;
                            currentEventDetail.IsEnabled = false;
                            _eventDetailRepository.Save(currentEventDetail);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    EventDetail eventDetail = new EventDetail();
                    eventDetail.Id = command.EventDetailId;
                    eventDetail.AltId = Guid.NewGuid();
                    eventDetail.EventId = command.EventId;
                    eventDetail.Description = command.Description;
                    eventDetail.IsEnabled = true;
                    eventDetail.Name = ((command.Title != null && command.Title != "") ? command.Title : "place");
                    eventDetail.VenueId = venue.Id;
                    eventDetail.CreatedBy = command.ModifiedBy;
                    eventDetail.CreatedUtc = DateTime.Now;
                    eventDetail.UpdatedBy = command.ModifiedBy;
                    eventDetail.ModifiedBy = command.ModifiedBy;
                    eventDetail.UpdatedUtc = DateTime.Now;
                    eventDetail.StartDateTime = DateTime.Now;
                    eventDetail.EndDateTime = DateTime.UtcNow.AddYears(10);
                    eventDetail.GroupId = 1;
                    _eventDetailRepository.Save(eventDetail);
                }
                var singleEventDetail = eventdetailsaved.FirstOrDefault();
                if (singleEventDetail != null)
                {
                    foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in eventdetailsaved)
                    {
                        if (currentEventDetail.VenueId != venue.Id)
                        {
                            currentEventDetail.VenueId = venue.Id;
                            _eventDetailRepository.Save(currentEventDetail);
                        }
                    }
                    eventDetailId = singleEventDetail.Id;
                }
            }

            if (eventDetailId != 0 && command.ParentCategoryId == 98 && !string.IsNullOrEmpty(command.TimeZone) && !string.IsNullOrEmpty(command.TimeZoneAbbreviation))
            {
                var eventAttributes = new EventAttribute
                {
                    TimeZoneAbbreviation = command.TimeZoneAbbreviation,
                    EventDetailId = eventDetailId,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true,
                    TimeZone = command.TimeZone
                };
                _eventAttributeRepository.Save(eventAttributes);
            }
            List<String> tilesImageList = new List<string>();
            tilesImageList = command.TilesSliderImages.Split(",").ToList<String>();

            List<String> descpagebanneImageList = new List<string>();
            descpagebanneImageList = command.DescpagebannerImages.Split(",").ToList<String>();

            List<String> inventorypagebannerImageList = new List<string>();
            inventorypagebannerImageList = command.InventorypagebannerImage.Split(",").ToList<String>();

            List<String> galleryImagesList = new List<string>();
            galleryImagesList = command.GalleryImages.Split(",").ToList<String>();

            List<String> placemapImagesList = new List<string>();
            placemapImagesList = command.PlacemapImages.Split(",").ToList<String>();

            List<String> timelineImagesList = new List<string>();
            timelineImagesList = command.TimelineImages.Split(",").ToList<String>();

            List<String> archdetailImagesList = new List<string>();
            archdetailImagesList = command.ArchdetailImages.Split(",").ToList<String>();

            var getGallaryImages = _eventGalleryImageRepository.GetByEventId(command.EventId).ToList();
            foreach (FIL.Contracts.DataModels.EventGalleryImage gallaryImages in getGallaryImages)
            {
                _eventGalleryImageRepository.Delete(gallaryImages);
            }
            galleryImagesList = galleryImagesList.Take(galleryImagesList.Count() - 1).ToList();
            foreach (string gallaryImages in galleryImagesList)
            {
                EventGalleryImage eventGalleryImage = new EventGalleryImage();
                eventGalleryImage.Id = 0;
                eventGalleryImage.AltId = Guid.NewGuid();
                eventGalleryImage.Name = "a.jpg";
                eventGalleryImage.EventId = command.EventId;
                eventGalleryImage.CreatedUtc = DateTime.Now;
                eventGalleryImage.UpdatedBy = command.ModifiedBy;
                eventGalleryImage.ModifiedBy = command.ModifiedBy;
                eventGalleryImage.UpdatedUtc = DateTime.Now;
                eventGalleryImage.IsEnabled = true;
                _eventGalleryImageRepository.Save(eventGalleryImage);
            }
            return new LocationCommandResult() { Id = eventDetailId, AltId = venue.AltId };
        }
    }
}