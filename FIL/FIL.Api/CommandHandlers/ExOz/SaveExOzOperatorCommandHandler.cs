﻿using FIL.Api.Repositories;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations.ExOz;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ExOz
{
    public class SaveExOzOperatorCommandHandler : BaseCommandHandlerWithResult<SaveExOzOperatorCommand, SaveExOzOperatorCommandResult>
    {
        private readonly IExOzRegionRepository _exOzRegionRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IExOzOperatorRepository _exOzOperatorRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IExOzOperatorImageRepository _exOzOperatorImageRepository;

        private SaveExOzOperatorCommandResult updatedOperators = new SaveExOzOperatorCommandResult()
        {
            OperatorList = new List<ExOzOperator>()
        };

        public SaveExOzOperatorCommandHandler(IExOzOperatorRepository exOzOperatorRepository, ICityRepository cityRepository,
        IExOzRegionRepository exOzRegionRepository, IEventRepository eventRepository, IVenueRepository venueRepository, IExOzOperatorImageRepository exOzOperatorImageRepository, IMediator mediator)
            : base(mediator)
        {
            _exOzRegionRepository = exOzRegionRepository;
            _cityRepository = cityRepository;

            _exOzOperatorRepository = exOzOperatorRepository;
            _eventRepository = eventRepository;

            _venueRepository = venueRepository;
            _exOzOperatorImageRepository = exOzOperatorImageRepository;
        }

        protected override Task<ICommandResult> Handle(SaveExOzOperatorCommand command)
        {
            _exOzOperatorRepository.DisableAllExOzOperators();
            _exOzOperatorImageRepository.DisableAllExOzOperatorImages();

            UpdateAllOperators(command);
            return Task.FromResult<ICommandResult>(updatedOperators);
        }

        protected void UpdateAllOperators(SaveExOzOperatorCommand command)
        {
            List<string> apiOperatorNames = command.OperatorList.Select(w => w.Name).Distinct().ToList();

            var kzEvents = _eventRepository.GetByNames(apiOperatorNames);
            var exOzOperators = _exOzOperatorRepository.GetByNames(apiOperatorNames);
            foreach (var item in command.OperatorList)
            {
                ExOzOperator exOzOperator = exOzOperators.Where(w => w.OperatorId == item.Id).FirstOrDefault();
                Event kzEvent = kzEvents.Where(w => w.Name == item.Name).FirstOrDefault();

                ExOzRegion exOzRegion = _exOzRegionRepository.GetByUrlSegment(item.RegionUrlSegment);
                City kzCity = _cityRepository.GetByName(exOzRegion.Name);
                Venue kzVenue = _venueRepository.GetByName(item.Geolocations[0].Address);

                //Venue
                try
                {
                    Venue retVenue = UpdateVenue(item, kzVenue, kzCity.Id, command.ModifiedBy);
                    Event retEvent = UpdateEvent(item, kzEvent, command.ModifiedBy);
                    ExOzOperator retOperator = UpdateOperator(item, exOzOperator, retEvent.Id, retVenue.Id, command.ModifiedBy);
                    updatedOperators.OperatorList.Add(retOperator);
                    UpdateAllOperatorImages(item, retOperator.Id, command.ModifiedBy);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        protected Venue UpdateVenue(ExOzOperatorResponse item, Venue kzVenue, int kzCityId, Guid ModifiedBy)
        {
            string VenueName = "";
            if (item.Geolocations[0].Label == null)
                VenueName = item.Geolocations[0].Address;
            else
                VenueName = item.Geolocations[0].Label;
            //Venue
            Venue kzVenueInserted = new Venue();
            if (kzVenue == null)
            {
                Venue newKzVenue = new Venue
                {
                    AltId = Guid.NewGuid(),
                    Name = VenueName,
                    AddressLineOne = item.Geolocations[0].Address,
                    CityId = kzCityId,
                    Latitude = item.Geolocations[0].Latitude,
                    Longitude = item.Geolocations[0].Longitude,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true
                };
                kzVenueInserted = _venueRepository.Save(newKzVenue);
            }
            else
            {
                kzVenueInserted = kzVenue;
            }
            return kzVenueInserted;
        }

        protected Event UpdateEvent(ExOzOperatorResponse item, Event kzEvent, Guid ModifiedBy)
        {
            Event kzEventInserted = new Event();
            if (kzEvent == null)
            {
                var newKzEvent = new Event
                {
                    AltId = Guid.NewGuid(),
                    Name = item.Name,
                    EventCategoryId = 16,
                    EventTypeId = Contracts.Enums.EventType.Perennial,
                    Description = item.Description,
                    ClientPointOfContactId = 1,
                    FbEventId = null,
                    MetaDetails = null,
                    TermsAndConditions = item.Tips,
                    IsPublishedOnSite = true,
                    PublishedDateTime = DateTime.Now,
                    PublishedBy = null,
                    TestedBy = null,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true
                };
                kzEventInserted = _eventRepository.Save(newKzEvent);
            }
            else
            {
                kzEventInserted = kzEvent;
            }
            return kzEventInserted;
        }

        protected ExOzOperator UpdateOperator(ExOzOperatorResponse item, ExOzOperator exOzOperator, long kzEventId, int kzVenueId, Guid ModifiedBy)
        {
            ExOzOperator exOzOperatorInserted = new ExOzOperator();
            if (exOzOperator == null)
            {
                var newExOzOperator = new ExOzOperator
                {
                    OperatorId = item.Id,
                    Name = item.Name,
                    PublicName = item.PublicName,
                    UrlSegment = item.UrlSegment,
                    CanonicalRegionUrlSegment = item.CanonicalRegionUrlSegment,
                    RegionId = item.RegionId,
                    EventId = kzEventId,
                    VenueId = kzVenueId,
                    Title = item.Title,
                    Description = item.Description,
                    Summary = item.Summary,
                    Tips = item.Tips,
                    Address = item.Address,
                    Phone = item.Phone,
                    Rating = item.Rating,
                    Quantity = item.Quantity,
                    Timestamp = item.Timestamp,
                    GeoLocationName = item.Geolocations[0].Address,
                    Latitude = item.Geolocations[0].Latitude,
                    Longitude = item.Geolocations[0].Longitude,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true
                };
                exOzOperatorInserted = _exOzOperatorRepository.Save(newExOzOperator);
            }
            else
            {
                exOzOperator.IsEnabled = true;
                exOzOperator.ModifiedBy = ModifiedBy;
                exOzOperatorInserted = _exOzOperatorRepository.Save(exOzOperator);
            }
            return exOzOperatorInserted;
        }

        protected void UpdateAllOperatorImages(ExOzOperatorResponse item, long OperatorId, Guid ModifiedBy)
        {
            List<ExOzOperatorImage> exOzOperatorImages =
                _exOzOperatorImageRepository.GetByOperatorId(OperatorId).ToList();
            foreach (var img in item.Images)
            {
                ExOzOperatorImage existingExOzOperatorImage = exOzOperatorImages.Where(w => w.ImageURL == img).FirstOrDefault();
                if (existingExOzOperatorImage == null)
                {
                    ExOzOperatorImage newImage = new ExOzOperatorImage()
                    {
                        ImageURL = img,
                        OperatorId = OperatorId,
                        IsEnabled = true,
                        ModifiedBy = ModifiedBy,
                    };
                    _exOzOperatorImageRepository.Save(newImage);
                }
                else
                {
                    existingExOzOperatorImage.IsEnabled = true;
                    _exOzOperatorImageRepository.Save(existingExOzOperatorImage);
                }
            }
        }
    }
}