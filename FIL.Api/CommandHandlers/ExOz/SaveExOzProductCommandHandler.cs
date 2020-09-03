using FIL.Api.Repositories;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations.ExOz;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzGeolocationResponse;

namespace FIL.Api.CommandHandlers.ExOz
{
    public class SaveExOzProductCommandHandler : BaseCommandHandlerWithResult<SaveExOzProductCommand, SaveExOzProductCommandResult>
    {
        private readonly IExOzOperatorRepository _exOzOperatorRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IExOzProductRepository _exOzProductRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IExOzProductImageRepository _exOzProductImageRepository;
        private readonly IExOzProductHighlightRepository _exOzProductHighlightRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IExOzRegionRepository _exOzRegionRepository;
        private readonly ICityRepository _cityRepository;

        private SaveExOzProductCommandResult updatedProducts = new SaveExOzProductCommandResult()
        {
            ProductList = new List<ExOzProduct>()
        };

        public SaveExOzProductCommandHandler(IExOzProductRepository exOzProductRepository, IEventRepository eventRepository,
        IExOzOperatorRepository exOzOperatorRepository, IEventDetailRepository eventDetailRepository, IVenueRepository venueRepository, IExOzProductImageRepository exOzProductImageRepository, IExOzProductHighlightRepository exOzProductHighlightRepository, ICityRepository cityRepository,
        IExOzRegionRepository exOzRegionRepository, IMediator mediator)
            : base(mediator)
        {
            _exOzOperatorRepository = exOzOperatorRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;

            _exOzRegionRepository = exOzRegionRepository;
            _cityRepository = cityRepository;

            _exOzProductRepository = exOzProductRepository;
            _eventDetailRepository = eventDetailRepository;

            _exOzProductImageRepository = exOzProductImageRepository;
            _exOzProductHighlightRepository = exOzProductHighlightRepository;
        }

        protected override Task<ICommandResult> Handle(SaveExOzProductCommand command)
        {
            _exOzProductRepository.DisableAllExOzProducts();
            _exOzProductImageRepository.DisableAllExOzProductImages();
            _exOzProductHighlightRepository.DisableAllExOzProductHighlights();

            UpdateAllProducts(command);
            return Task.FromResult<ICommandResult>(updatedProducts);
        }

        protected void UpdateAllProducts(SaveExOzProductCommand command)
        {
            int i = 0;
            List<string> apiProductNames = command.ProductList.Select(w => w.Name).Distinct().ToList();

            var FilEventDetails = _eventDetailRepository.GetByNames(apiProductNames);
            var exOzProducts = _exOzProductRepository.GetByNames(apiProductNames);

            foreach (var item in command.ProductList)
            {
                i++;
                ExOzProduct exOzProduct = exOzProducts.Where(w => w.ProductId == item.Id).FirstOrDefault();
                EventDetail FilEventDetail = FilEventDetails.Where(w => w.Name == item.Name).FirstOrDefault();

                ExOzOperator exOzOperator = _exOzOperatorRepository.GetByUrlSegment(item.OperatorUrlSegment);
                Event FilEvent = _eventRepository.GetAllByName(exOzOperator.Name).FirstOrDefault();
                //Venue FilVenue = _venueRepository.GetByName(item.Geolocations[0].Address);
                //if (FilVenue == null)
                //{
                //    ExOzRegion exOzRegion = _exOzRegionRepository.GetByUrlSegment(exOzOperator.CanonicalRegionUrlSegment);
                //    City FilCity = _cityRepository.GetByName(exOzRegion.Name);
                //    FilVenue = UpdateVenue(item.Geolocations[0], FilVenue, FilCity.Id, command.ModifiedBy);
                //}
                try
                {
                    EventDetail retEventDetail = UpdateEventDetail(item.Name, FilEventDetail, FilEvent.Id, (int)exOzOperator.VenueId, command.ModifiedBy);
                    ExOzProduct retProduct = UpdateProduct(item, exOzProduct, retEventDetail.Id, (int)exOzOperator.VenueId, command.ModifiedBy);
                    updatedProducts.ProductList.Add(retProduct);
                    UpdateProductImages(item, retEventDetail.Id, retProduct.Id, command.ModifiedBy);
                    UpdateProductHighlights(item, retEventDetail.Id, retProduct.Id, command.ModifiedBy);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        protected Venue UpdateVenue(Geolocation item, Venue FilVenue, int FilCityId, Guid ModifiedBy)
        {
            string VenueName = "";
            if (item.Label == null)
                VenueName = item.Address;
            else
                VenueName = item.Label;
            //Venue
            Venue FilVenueInserted = new Venue();
            if (FilVenue == null)
            {
                Venue newFilVenue = new Venue
                {
                    AltId = Guid.NewGuid(),
                    Name = VenueName,
                    AddressLineOne = item.Address,
                    CityId = FilCityId,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true
                };
                FilVenueInserted = _venueRepository.Save(newFilVenue);
            }
            else
            {
                FilVenueInserted = FilVenue;
            }
            return FilVenueInserted;
        }

        protected EventDetail UpdateEventDetail(string name, EventDetail FilEventDetail, long FilEventId, int FilVenueId, Guid ModifiedBy)
        {
            EventDetail FilEventDetailInserted = new EventDetail();
            if (FilEventDetail == null)
            {
                var newFilEventDetail = new EventDetail
                {
                    Name = name,
                    EventId = FilEventId,
                    VenueId = FilVenueId,
                    StartDateTime = DateTime.UtcNow,
                    EndDateTime = DateTime.UtcNow,
                    GroupId = 1,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                    MetaDetails = "",
                    HideEventDateTime = false,
                    CustomDateTimeMessage = "",
                };
                FilEventDetailInserted = _eventDetailRepository.Save(newFilEventDetail);
            }
            else
            {
                FilEventDetailInserted = FilEventDetail;
            }
            return FilEventDetailInserted;
        }

        protected ExOzProduct UpdateProduct(ExOzProductResponse item, ExOzProduct exOzProduct, long FilEventDetailId, int FilVenueId, Guid ModifiedBy)
        {
            ExOzProduct exOzProductInserted = new ExOzProduct();
            if (exOzProduct == null)
            {
                var newExOzProduct = new ExOzProduct
                {
                    ProductId = item.Id,
                    Name = item.Name,
                    UrlSegment = item.UrlSegment,
                    Summary = item.Summary,
                    OperatorId = item.OperatorId,
                    EventDetailId = FilEventDetailId,
                    VenueId = FilVenueId,
                    CanonicalRegionUrlSegment = item.CanonicalRegionUrlSegment,
                    BookingRequired = item.BookingRequired,
                    HandlerKey = item.HandlerKey,
                    Title = item.Title,
                    Timestamp = item.Timestamp,
                    OperatorName = item.OperatorName,
                    Description = item.Description,
                    MoreInfo = item.MoreInfo,
                    Tips = item.Tips,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                    HelpCode = item.HelpCode,
                    Timezone = item.Timezone,
                };
                exOzProductInserted = _exOzProductRepository.Save(newExOzProduct);
            }
            else
            {
                exOzProduct.IsEnabled = true;
                exOzProduct.ModifiedBy = ModifiedBy;
                exOzProductInserted = _exOzProductRepository.Save(exOzProduct);
            }
            return exOzProductInserted;
        }

        protected void UpdateProductImages(ExOzProductResponse product, long EventDetailId, long ProductId, Guid ModifiedBy)
        {
            List<ExOzProductImage> exOzProductImages =
                _exOzProductImageRepository.GetByProductId(product.Id).ToList();
            foreach (var img in product.Images)
            {
                ExOzProductImage exOzProductImage = exOzProductImages.Where(w => w.ImageURL == img).FirstOrDefault();
                if (exOzProductImage == null)
                {
                    ExOzProductImage newImage = new ExOzProductImage()
                    {
                        ImageURL = img,
                        ProductId = ProductId,
                        EventDetailId = EventDetailId,
                        IsEnabled = true,
                        ModifiedBy = ModifiedBy,
                    };
                    exOzProductImage = _exOzProductImageRepository.Save(newImage);
                }
                else
                {
                    exOzProductImage.IsEnabled = true;
                    exOzProductImage.ModifiedBy = ModifiedBy;
                    exOzProductImage = _exOzProductImageRepository.Save(exOzProductImage);
                }
            }
        }

        protected void UpdateProductHighlights(ExOzProductResponse product, long EventDetailId, long ProductId, Guid ModifiedBy)
        {
            List<ExOzProductHighlight> exOzProductHighlights =
                _exOzProductHighlightRepository.GetByProductId(product.Id).ToList();
            foreach (var highlight in product.Highlights)
            {
                ExOzProductHighlight exOzProductHighlight = exOzProductHighlights.Where(w => w.Highlight == highlight).FirstOrDefault();
                if (exOzProductHighlight == null)
                {
                    ExOzProductHighlight newHighlight = new ExOzProductHighlight()
                    {
                        Highlight = highlight,
                        ProductId = ProductId,
                        EventDetailId = EventDetailId,
                        IsEnabled = true,
                        ModifiedBy = ModifiedBy,
                    };
                    exOzProductHighlight = _exOzProductHighlightRepository.Save(newHighlight);
                }
                else
                {
                    exOzProductHighlight.IsEnabled = true;
                    exOzProductHighlight.ModifiedBy = ModifiedBy;
                    exOzProductHighlight = _exOzProductHighlightRepository.Save(exOzProductHighlight);
                }
            }
        }
    }
}