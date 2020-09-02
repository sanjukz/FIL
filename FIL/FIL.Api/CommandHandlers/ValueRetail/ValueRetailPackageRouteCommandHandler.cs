using FIL.Api.Integrations.ValueRetail;
using FIL.Api.Repositories;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Contracts.Commands.ValueRetail;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations.ValueRetail;
using FIL.Contracts.Models.ValueRetail.ShoppingPackage;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ValueRetail
{
    public class ValueRetailPackageRouteCommandHandler : BaseCommandHandler<ValueRetailPackageRouteCommand>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IMediator _mediator;
        private readonly IValueRetailVillageRepository _valueRetailVillageRepository;
        private readonly IValueRetailPackageRouteRepository _valueRetailPackageRouteRepository;
        private readonly IValueRetailPackageReturnRepository _valueRetailPackageReturnRepository;
        private readonly IValueRetailAPI _valueRetailAPI;

        public ValueRetailPackageRouteCommandHandler(
            ILogger logger,
            ISettings settings,
            IMediator mediator,
            IValueRetailVillageRepository valueRetailVillageRepository,
            IValueRetailPackageRouteRepository valueRetailPackageRouteRepository,
            IValueRetailPackageReturnRepository valueRetailPackageReturnRepository,
            IValueRetailAPI valueRetailAPI
           )
           : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _mediator = mediator;
            _valueRetailVillageRepository = valueRetailVillageRepository;
            _valueRetailPackageRouteRepository = valueRetailPackageRouteRepository;
            _valueRetailPackageReturnRepository = valueRetailPackageReturnRepository;
            _valueRetailAPI = valueRetailAPI;
        }

        protected override async Task Handle(ValueRetailPackageRouteCommand command)
        {
            var villages = _valueRetailVillageRepository.GetAll();
            foreach (var village in villages)
            {
                await FetchAndSavePackageRoutes(village);
            }
        }

        public async Task FetchAndSavePackageRoutes(ValueRetailVillage village)
        {
            var httpRequest = new ValueRetailCommanRequestModel
            {
                from = DateTime.UtcNow,
                cultureCode = village.CultureCode,
                villageCode = village.VillageCode
            };

            var responseString = await _valueRetailAPI.GetValueRetailAPIData(httpRequest, "Routes", "ShoppingPackage");

            ShoppingPackageRouteResponse responseData = new ShoppingPackageRouteResponse();
            try
            {
                responseData = Mapper<ShoppingPackageRouteResponse>.MapFromJson(responseString.Result);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }

            foreach (var package in responseData.Packages)
            {
                var currentPackage = _valueRetailPackageRouteRepository.GetOneByPackageId(package.Package.PackageID);
                if (currentPackage == null)
                {
                    foreach (var journey in package.Journeys)
                    {
                        foreach (var route in journey.Routes)
                        {
                            try
                            {
                                currentPackage = _valueRetailPackageRouteRepository.Save(new ValueRetailPackageRoute
                                {
                                    VillageId = village.Id,
                                    PackageId = package.Package.PackageID,
                                    JourneyType = journey.JourneyType,
                                    RouteId = route.RouteId,
                                    DepartureTime = route.DepartureTime,
                                    LinkedRouteId = route.LinkedRouteId,
                                    ReturnTime = route.ReturnTime,
                                    Name = route.Name,
                                    LocationId = route.LocationId,
                                    LocationName = route.LocationName,
                                    LocationAddress = route.LocationAddress,
                                    StopId = route.StopId,
                                    StopOrder = route.StopOrder,
                                    Latitude = route.Latitude,
                                    Longitude = route.Longitude,
                                    AdultPrice = route.Prices.Adult == null ? 0 : Convert.ToDecimal(route.Prices.Adult.Price),
                                    ChildrenPrice = route.Prices.Children == null ? 0 : Convert.ToDecimal(route.Prices.Children.Price),
                                    FamilyPrice = route.Prices.Family == null ? 0 : Convert.ToDecimal(route.Prices.Family.Price),
                                    InfantPrice = route.Prices.Infant == null ? 0 : Convert.ToDecimal(route.Prices.Infant.Price),
                                    CreatedUtc = DateTime.UtcNow,
                                    CreatedBy = Guid.NewGuid(),
                                });

                                if (route.ReturnStops != null && route.ReturnStops.Count > 0)
                                {
                                    foreach (var returnStop in route.ReturnStops)
                                    {
                                        var currentReturnStop = _valueRetailPackageReturnRepository.Save(new ValueRetailPackageReturn
                                        {
                                            ValueRetailPackageRouteId = currentPackage.Id,
                                            RouteId = returnStop.RouteId,
                                            StopId = returnStop.StopId,
                                            StopOrder = returnStop.StopOrder,
                                            Name = returnStop.Name,
                                            LocationId = returnStop.LocationId,
                                            LocationName = returnStop.LocationName,
                                            LocationAddress = returnStop.LocationAddress,
                                            ReturnTime = returnStop.ReturnTime,
                                            Latitude = returnStop.Latitude,
                                            Longitude = returnStop.Longitude,
                                            CreatedUtc = DateTime.UtcNow,
                                            CreatedBy = Guid.NewGuid(),
                                        });
                                    }
                                }
                            }
                            catch (TaskCanceledException ex)
                            {
                                _logger.Log(LogCategory.Error, ex);
                            }
                            catch (Exception ex)
                            {
                                _logger.Log(LogCategory.Error, new Exception("Failed to save Value Retail Package Routes in Db", ex));
                            }
                        }
                    }
                }
            }
        }
    }
}