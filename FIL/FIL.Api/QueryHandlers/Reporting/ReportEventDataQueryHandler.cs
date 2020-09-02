using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.ReportEventData
{
    public class ReportEventDataQueryHandler : IQueryHandler<ReportEventDataQuery, ReportEventDataQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;

        public ReportEventDataQueryHandler(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public ReportEventDataQueryResult Handle(ReportEventDataQuery query)
        {
            FIL.Contracts.DataModels.ReportEventData reportEventData = new FIL.Contracts.DataModels.ReportEventData();
            List<FIL.Contracts.Models.Event> events = new List<FIL.Contracts.Models.Event>();
            List<FIL.Contracts.Models.EventDetail> eventDetails = new List<FIL.Contracts.Models.EventDetail>();
            List<FIL.Contracts.Models.EventAttribute> eventAttributes = new List<FIL.Contracts.Models.EventAttribute>();
            List<EventTicketDetail> eventTicketDetails = new List<EventTicketDetail>();
            List<FIL.Contracts.Models.TicketCategory> ticketCategories = new List<FIL.Contracts.Models.TicketCategory>();
            List<EventTicketAttribute> eventTicketAttributes = new List<EventTicketAttribute>();
            List<TicketFeeDetail> ticketFeeDetails = new List<TicketFeeDetail>();

            try
            {
                //reportEventData = _reportingRepository.GetReportEventData(query);
                //events = AutoMapper.Mapper.Map<List<Kz.Contracts.Models.Event>>(reportEventData.Event);
                //eventDetails = AutoMapper.Mapper.Map<List<Kz.Contracts.Models.EventDetail>>(reportEventData.EventDetail);
                //eventAttributes = AutoMapper.Mapper.Map<List<EventAttribute>>(reportEventData.EventAttribute);
                //eventTicketDetails = AutoMapper.Mapper.Map<List<EventTicketDetail>>(reportEventData.EventTicketDetail);
                //ticketCategories = AutoMapper.Mapper.Map<List<Kz.Contracts.Models.TicketCategory>>(reportEventData.TicketCategory);
                //eventTicketAttributes = AutoMapper.Mapper.Map<List<EventTicketAttribute>>(reportEventData.EventTicketAttribute);
                //ticketFeeDetails = AutoMapper.Mapper.Map<List<TicketFeeDetail>>(reportEventData.TicketFeeDetail);
                //List<Venue> venues = AutoMapper.Mapper.Map<List<Venue>>(reportEventData.Venue);
                //List<City> cities = AutoMapper.Mapper.Map<List<City>>(reportEventData.City);
                //List<State> states = AutoMapper.Mapper.Map<List<State>>(reportEventData.State);
                //List<Country> countries = AutoMapper.Mapper.Map<List<Country>>(reportEventData.Country);

                return new ReportEventDataQueryResult
                {
                    Event = events,
                    EventDetail = eventDetails,
                    EventAttribute = eventAttributes,
                    EventTicketDetail = eventTicketDetails,
                    TicketCategory = ticketCategories,
                    EventTicketAttribute = eventTicketAttributes,
                    TicketFeeDetail = ticketFeeDetails,
                    //Venue = venues,
                    //City = cities,
                    //State = states,
                    //Country = countries,
                    Venue = new List<Venue>(),
                    City = new List<City>(),
                    State = new List<State>(),
                    Country = new List<Country>()
                };
            }
            catch (System.Exception ex)
            {
                return new ReportEventDataQueryResult
                {
                    Event = events,
                    EventDetail = eventDetails,
                    EventAttribute = eventAttributes,
                    EventTicketDetail = eventTicketDetails,
                    TicketCategory = ticketCategories,
                    EventTicketAttribute = eventTicketAttributes,
                    TicketFeeDetail = ticketFeeDetails,
                    Venue = new List<Venue>(),
                    City = new List<City>(),
                    State = new List<State>(),
                    Country = new List<Country>()
                };
            }
        }
    }
}