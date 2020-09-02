using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Queries.PlaceInventory;
using FIL.Contracts.QueryResults.PlaceInventory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.PlaceInventory
{
    public class GetPlaceInventoryQueryHandler : IQueryHandler<GetPlaceInventoryQuery, GetPlaceInventoryQueryResult>
    {
        private readonly IEventTicketDetailRepository _eventTicketDetail;
        private readonly IEventTicketAttributeRepository _eventTicketAttribute;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IRefundPolicyRepository _refundPolicyRepository;
        private readonly IPlaceTicketRedemptionDetailRepository _placeTicketRedemptionDetailRepository;
        private readonly IPlaceDocumentTypeMappingRepository _placeDocumentTypeMappingRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly IPlaceCustomerDocumentTypeMappingRepository _placeCustomerDocumentTypeMappingRepository;
        private readonly ICustomerDocumentTypeRepository _customerDocumentTypeRepository;
        private readonly IPlaceHolidayDatesRepository _placeHolidydates;
        private readonly IPlaceWeekOffRepository _placeWeekOffRepository;
        private readonly IEventTicketDetailTicketCategoryTypeMappingRepository _eventTicketDetailTicketCategoryTypeMappingRepository;
        private readonly ICustomerInformationRepository _customerInformationRepository;
        private readonly IEventCustomerInformationMappingRepository _eventCustomerInformationMappingRepository;
        private readonly IDaysRepository _daysRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IDayTimeMappingsRepository _dayTimeMappingsRepository;
        private readonly IPlaceSeasonDetailsRepository _placeSeasonDetailsRepository;
        private readonly ISeasonDayTimeMappingsRepository _seasonDayTimeMappingsRepository;
        private readonly ISeasonDaysMappingsRepository _seasonDaysMappingsRepository;
        private readonly IPlaceSpecialDayTimeMappingsRepository _placeSpecialDayTimeMappingsRepository;
        private readonly ICalendarProvider _calendarProvider;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;

        public GetPlaceInventoryQueryHandler(
            IEventTicketDetailRepository eventTicketDetail,
             IPlaceHolidayDatesRepository placeHolidayDatesRepository,
            IEventTicketAttributeRepository eventTicketAttribute,
            ITicketCategoryRepository ticketCategoryRepository,
            IPlaceTicketRedemptionDetailRepository placeTicketRedemptionDetailRepository,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository,
            IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository,
            IRefundPolicyRepository refundPolicyRepository,
            IPlaceCustomerDocumentTypeMappingRepository placeCustomerDocumentTypeMappingRepository,
            IPlaceDocumentTypeMappingRepository placeDocumentTypeMappingRepository,
            ICustomerDocumentTypeRepository customerDocumentTypeRepository,
            ICustomerInformationRepository customerInformationRepository,
            IEventCustomerInformationMappingRepository eventCustomerInformationMappingRepository,
            IEventTicketDetailTicketCategoryTypeMappingRepository eventTicketDetailTicketCategoryTypeMappingRepository,
            IDaysRepository daysRepository,
            IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IDayTimeMappingsRepository dayTimeMappingsRepository,
            IPlaceSeasonDetailsRepository placeSeasonDetailsRepository,
            ISeasonDayTimeMappingsRepository seasonDayTimeMappingsRepository,
            ISeasonDaysMappingsRepository seasonDaysMappingsRepository,
            IPlaceSpecialDayTimeMappingsRepository placeSpecialDayTimeMappingsRepository,
            ICalendarProvider calendarProvider,
            IEventAttributeRepository eventAttributeRepository,
            IPlaceWeekOffRepository placeWeekOffRepository, ITicketFeeDetailRepository ticketFeeDetailRepository)
        {
            _eventTicketDetail = eventTicketDetail;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _placeDocumentTypeMappingRepository = placeDocumentTypeMappingRepository;
            _eventRepository = eventRepository;
            _eventTicketAttribute = eventTicketAttribute;
            _refundPolicyRepository = refundPolicyRepository;
            _placeTicketRedemptionDetailRepository = placeTicketRedemptionDetailRepository;
            _placeCustomerDocumentTypeMappingRepository = placeCustomerDocumentTypeMappingRepository;
            _placeHolidydates = placeHolidayDatesRepository;
            _placeWeekOffRepository = placeWeekOffRepository;
            _customerInformationRepository = customerInformationRepository;
            _eventTicketDetailTicketCategoryTypeMappingRepository = eventTicketDetailTicketCategoryTypeMappingRepository;
            _customerDocumentTypeRepository = customerDocumentTypeRepository;
            _eventCustomerInformationMappingRepository = eventCustomerInformationMappingRepository;
            _daysRepository = daysRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _dayTimeMappingsRepository = dayTimeMappingsRepository;
            _placeSeasonDetailsRepository = placeSeasonDetailsRepository;
            _seasonDayTimeMappingsRepository = seasonDayTimeMappingsRepository;
            _seasonDaysMappingsRepository = seasonDaysMappingsRepository;
            _placeSpecialDayTimeMappingsRepository = placeSpecialDayTimeMappingsRepository;
            _calendarProvider = calendarProvider;
            _eventAttributeRepository = eventAttributeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
        }

        public GetPlaceInventoryQueryResult Handle(GetPlaceInventoryQuery query)
        {
            try
            {
                var eventData = _eventRepository.GetByAltId(query.PlaceAltId);
                var eventModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.Event>(eventData);

                List<string> TicketValidityTypes = new List<string>();
                List<string> DeliveryTypes = new List<string>();

                foreach (FIL.Contracts.Enums.TicketValidityTypes ticketValidity in Enum.GetValues(typeof(FIL.Contracts.Enums.TicketValidityTypes)))
                {
                    TicketValidityTypes.Add(ticketValidity.ToString());
                }

                foreach (FIL.Contracts.Enums.DeliveryTypes DeliveryType in Enum.GetValues(typeof(FIL.Contracts.Enums.DeliveryTypes)))
                {
                    DeliveryTypes.Add(DeliveryType.ToString());
                }

                var eventDetail = _eventDetailRepository.GetAllByEventId(eventData.Id);
                var eventDetailModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(eventDetail).Where(s => s.IsEnabled == true).ToList();

                var eventTicketDetils = _eventTicketDetail.GetByEventDetailId(eventDetail.ElementAt(0).Id).Where(s => s.IsEnabled == true);
                var eventAttribute = _eventAttributeRepository.GetByEventDetailId(eventDetail.FirstOrDefault().Id);
                var EventTicketDetailTicketCategoryTypeMappingDataModel = _eventTicketDetailTicketCategoryTypeMappingRepository.GetByEventTicketDetails(eventTicketDetils.Select(s => s.Id).ToList()).Where(s => s.IsEnabled == true);
                var EventTicketDetailTicketCategoryTypeMappingModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventTicketDetailTicketCategoryTypeMapping>>(EventTicketDetailTicketCategoryTypeMappingDataModel);

                var ticketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetils.Select(s => s.TicketCategoryId)).Where(s => s.IsEnabled == true);

                var placeCustomerDocumentTypes = _placeCustomerDocumentTypeMappingRepository.GetAllByEventId(eventData.Id);
                var placeCustomerDocumentTypeModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.PlaceCustomerDocumentTypeMapping>>(placeCustomerDocumentTypes);

                var placeTicketRedemptionDetails = _placeTicketRedemptionDetailRepository.GetAllByEventDetailIds(eventDetail.Select(s => s.Id));
                var placeTicketRedemptionDetailModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.PlaceTicketRedemptionDetail>>(placeTicketRedemptionDetails);

                var placeDeliveryTypes = _eventDeliveryTypeDetailRepository.GetByEventDetailId(eventDetail.ElementAt(0).Id);
                var placeDeliveryTypesModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDeliveryTypeDetail>>(placeDeliveryTypes);

                var customerDoumentType = _customerDocumentTypeRepository.GetAll();
                var customerDoumentTypeModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.CustomerDocumentType>>(customerDoumentType);

                var placeHolidayDates = _placeHolidydates.GetAllByEventId(eventData.Id);
                var placeHolidayDatesModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.PlaceHolidayDate>>(placeHolidayDates);

                var placeWeekOffs = _placeWeekOffRepository.GetAllByEventId(eventData.Id);
                var placeWeekOffsModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.PlaceWeekOff>>(placeWeekOffs);

                var customerInformationMapping = _customerInformationRepository.GetAll();
                var customerInformationMappingModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.CustomerInformation>>(customerInformationMapping);

                var eventCustomerInformationMapping = _eventCustomerInformationMappingRepository.GetAllByEventID(eventModel.Id).ToList();
                List<FIL.Contracts.Models.EventCustomerInformationMapping> eventCustomerInformationMappingModel = new List<Contracts.Models.EventCustomerInformationMapping>(); ;
                if (eventCustomerInformationMapping.Count() > 0)
                {
                    eventCustomerInformationMappingModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventCustomerInformationMapping>>(eventCustomerInformationMapping);
                }
                List<FIL.Contracts.DataModels.EventTicketDetail> etdList = new List<FIL.Contracts.DataModels.EventTicketDetail>();

                foreach (FIL.Contracts.DataModels.EventTicketDetail currentTicketCat in eventTicketDetils.ToList())
                { /*--------------- Add those ETD whose eventticketdetail and ETA exists */
                    var etaData = _eventTicketAttribute.GetByEventTicketDetailIdFeelAdmin(currentTicketCat.Id);
                    if (etaData != null)
                    {
                        etdList.Add(currentTicketCat);
                    }
                }

                var ticketCategoryContainer = etdList.Select(td =>
                {
                    try
                    {
                        var ticketDetailModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.EventTicketDetail>(td);
                        var ticketCategory = _ticketCategoryRepository.Get((int)td.TicketCategoryId);
                        var ticketCategoryModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.TicketCategory>(ticketCategory);
                        var eventTicketAttributes = _eventTicketAttribute.GetByEventTicketDetailIdFeelAdmin(td.Id);
                        var eventTicketAttributeModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.EventTicketAttribute>(eventTicketAttributes);
                        var TicketCategoryTypeTypeMapping = _eventTicketDetailTicketCategoryTypeMappingRepository.GetByEventTicketDetail(td.Id);
                        var ticketFeeDetail = _ticketFeeDetailRepository.GetAllByEventTicketAttributeId(eventTicketAttributeModel.Id);
                        var ticketFeeDetailModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TicketFeeDetail>>(ticketFeeDetail);

                        return new TicketCategoryInfo
                        {
                            TicketCategory = ticketCategoryModel,
                            EventTicketAttribute = eventTicketAttributeModel,
                            EventTicketDetail = ticketDetailModel,
                            TicketCategorySubTypeId = (TicketCategoryTypeTypeMapping != null ? TicketCategoryTypeTypeMapping.TicketCategorySubTypeId : 1),
                            TicketCategoryTypeId = (TicketCategoryTypeTypeMapping != null ? TicketCategoryTypeTypeMapping.TicketCategoryTypeId : 1),
                            TicketFeeDetails = ticketFeeDetailModel
                        };
                    }
                    catch (Exception e)
                    {
                        return new TicketCategoryInfo
                        {
                        };
                    }
                }).ToList();

                var data = _calendarProvider.GetCalendarData(eventData.Id);

                return new GetPlaceInventoryQueryResult
                {
                    Event = eventModel,
                    EventDetails = eventDetailModel,
                    TicketCategoryContainer = ticketCategoryContainer,
                    PlaceCustomerDocumentTypeMappings = placeCustomerDocumentTypeModel,
                    DeliveryTypes = DeliveryTypes,
                    eventDeliveryTypeDetails = placeDeliveryTypesModel,
                    EventTicketDetailTicketCategoryTypeMappings = EventTicketDetailTicketCategoryTypeMappingModel,
                    PlaceTicketRedemptionDetails = placeTicketRedemptionDetailModel,
                    TicketValidityTypes = TicketValidityTypes,
                    CustomerDocumentTypes = customerDoumentTypeModel,
                    PlaceHolidayDates = placeHolidayDatesModel,
                    PlaceWeekOffs = placeWeekOffsModel,
                    CustomerInformations = customerInformationMappingModel,
                    EventCustomerInformationMappings = eventCustomerInformationMappingModel,
                    RegularTimeModel = data.RegularTimeModel,
                    SeasonTimeModel = data.SeasonTimeModel,
                    SpecialDayModel = data.SpecialDayModel,
                    EventAttribute = eventAttribute
                };
            }
            catch (Exception e)
            {
                return new GetPlaceInventoryQueryResult
                {
                };
            }
        }
    }
}