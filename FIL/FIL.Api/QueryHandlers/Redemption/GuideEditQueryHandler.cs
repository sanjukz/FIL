using FIL.Api.Repositories;
using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.QueryResults.Redemption;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Redemption
{
    public class GuideEditQueryHandler : IQueryHandler<GuideEditQuery, GuideEditQueryResult>
    {
        private readonly IGuideDetailsRepository _GuideDetailsRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IGuidePlaceMappingsRepository _GuidePlaceMappingsRepository;
        private readonly IGuideServicesRepository _GuideServicesRepository;
        private readonly IMasterFinanceDetailsRepository _MasterFinanceDetailsRepository;
        private readonly IGuideDocumentMappingsRepository _GuideDocumentMappingsRepository;
        private readonly IGuideFinanceMappingsRepository _GuideFinanceMappingsRepository;
        private readonly IServicesRepository _servicesRepository;
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IEventRepository _eventRepository;
        private readonly FIL.Logging.ILogger _logger;

        public GuideEditQueryHandler(IGuideDetailsRepository GuideDetailsRepository,
        IUserRepository UserRepository,
        IGuidePlaceMappingsRepository GuidePlaceMappingsRepository,
        IGuideServicesRepository GuideServicesRepository,
        IMasterFinanceDetailsRepository MasterFinanceDetailsRepository,
        IGuideDocumentMappingsRepository GuideDocumentMappingsRepository,
        IGuideFinanceMappingsRepository GuideFinanceMappingsRepository,
        IServicesRepository ServicesRepository,
        IUserAddressDetailRepository UserAddressDetailRepository,
        ICityRepository cityRepository,
        IStateRepository stateRepository,
        ICountryRepository countryRepository,
        IEventRepository eventRepository,
        FIL.Logging.ILogger logger)
        {
            _GuideDetailsRepository = GuideDetailsRepository;
            _UserRepository = UserRepository;
            _GuidePlaceMappingsRepository = GuidePlaceMappingsRepository;
            _GuideServicesRepository = GuideServicesRepository;
            _MasterFinanceDetailsRepository = MasterFinanceDetailsRepository;
            _GuideDocumentMappingsRepository = GuideDocumentMappingsRepository;
            _GuideFinanceMappingsRepository = GuideFinanceMappingsRepository;
            _servicesRepository = ServicesRepository;
            _userAddressDetailRepository = UserAddressDetailRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public GuideEditQueryResult Handle(GuideEditQuery query)
        {
            try
            {
                var user = _UserRepository.GetByAltId(query.UserId);
                var guideDetails = _GuideDetailsRepository.GetByUserId(user.Id);
                var guidePlaceMappings = _GuidePlaceMappingsRepository.GetAllByGuideId(guideDetails.Id);
                var guidePlaces = _eventRepository.GetByEventIds(guidePlaceMappings.Select(t => t.EventId));
                var guideServices = _GuideServicesRepository.GetAllByGuideId(guideDetails.Id);
                var services = _servicesRepository.GetAllByIds(guideServices.Select(s => s.ServiceId).ToList());
                var guideFinanceDetails = _GuideFinanceMappingsRepository.GetAllByGuideId(guideDetails.Id).FirstOrDefault();
                var masterFinanceDetails = _MasterFinanceDetailsRepository.Get(guideFinanceDetails.MasterFinanceDetailId);
                var userAddressDetails = _userAddressDetailRepository.GetByUserId(user.Id).FirstOrDefault();
                var guideDocumentMappings = _GuideDocumentMappingsRepository.GetAllByGuideId(guideDetails.Id);
                var residentCity = _cityRepository.Get(Convert.ToInt32(userAddressDetails.CityId));
                var residentState = _stateRepository.Get(residentCity.StateId);
                var residentCountry = _countryRepository.Get(residentState.CountryId);
                var finacialState = _stateRepository.Get(masterFinanceDetails.StateId);

                var userAddressMapping = new UserAddressDetailMapping
                {
                    ResidentialCity = AutoMapper.Mapper.Map<FIL.Contracts.Models.City>(residentCity),
                    ResidentialState = AutoMapper.Mapper.Map<FIL.Contracts.Models.State>(residentState),
                    ResidentialCountry = AutoMapper.Mapper.Map<FIL.Contracts.Models.Country>(residentCountry),
                    FinancialState = AutoMapper.Mapper.Map<FIL.Contracts.Models.State>(finacialState)
                };

                return new GuideEditQueryResult
                {
                    UserAddressDetail = userAddressDetails,
                    UserAddressDetailMapping = userAddressMapping,
                    GuideDetail = guideDetails,
                    GuideFinanceMap = guideFinanceDetails,
                    MasterFinanceDetails = masterFinanceDetails,
                    GuidePlaceMappings = guidePlaceMappings.ToList(),
                    GuidePlaces = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(guidePlaces),
                    GuideServices = guideServices.ToList(),
                    User = user,
                    GuideDocumentMappings = guideDocumentMappings.ToList(),
                    Services = services.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new GuideEditQueryResult { };
            }
        }
    }
}