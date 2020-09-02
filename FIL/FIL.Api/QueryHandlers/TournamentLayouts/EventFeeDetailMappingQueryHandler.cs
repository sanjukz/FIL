using FIL.Api.Repositories;
using FIL.Contracts.Queries.TournamentLayout;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TournamentLayouts
{
    public class EventFeeDetailMappingQueryHandler : IQueryHandler<EventFeeTypeDetailsMappingQuery, EventFeeTypeDetailsMappingQueryResult>
    {
        private readonly IEventFeeTypeMappingRepository _eventFeeTypeMappingRepository;
        private readonly FIL.Logging.ILogger _logger;

        public EventFeeDetailMappingQueryHandler(IEventFeeTypeMappingRepository eventFeeTypeMappingRepository, FIL.Logging.ILogger logger)
        {
            _eventFeeTypeMappingRepository = eventFeeTypeMappingRepository;
            _logger = logger;
        }

        public EventFeeTypeDetailsMappingQueryResult Handle(EventFeeTypeDetailsMappingQuery query)
        {
            var eventFeeDetails = _eventFeeTypeMappingRepository.GetByEventId(query.EventId);
            try
            {
                if (eventFeeDetails.Any())
                {
                    List<FIL.Contracts.Models.TournamentLayouts.EventFeeTypeMappings> eventfeelist = new List<FIL.Contracts.Models.TournamentLayouts.EventFeeTypeMappings>();
                    foreach (var item in eventFeeDetails)
                    {
                        FIL.Contracts.Models.TournamentLayouts.EventFeeTypeMappings eventfee = new Contracts.Models.TournamentLayouts.EventFeeTypeMappings();
                        eventfee.id = item.Id;
                        eventfee.channel = (int)item.ChannelId;
                        eventfee.feeType = (int)item.FeeId;
                        eventfee.valueType = (int)item.ValueTypeId;
                        eventfee.Value = item.Value;
                        eventfeelist.Add(eventfee);
                    }
                    return new EventFeeTypeDetailsMappingQueryResult
                    {
                        eventFeeTypeMappings = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TournamentLayouts.EventFeeTypeMappings>>(eventfeelist)
                    };
                }
                else
                {
                    return new EventFeeTypeDetailsMappingQueryResult
                    {
                        eventFeeTypeMappings = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new EventFeeTypeDetailsMappingQueryResult
                {
                    eventFeeTypeMappings = null
                };
            }
        }
    }
}