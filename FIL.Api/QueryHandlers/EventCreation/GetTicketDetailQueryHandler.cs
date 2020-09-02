using FIL.Api.Repositories;
using FIL.Contracts.Queries.EventCreation;
using FIL.Contracts.QueryResults.EventCreation;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.EventCreation
{
    public class GetTicketDetailQueryHandler : IQueryHandler<GetticketDetailQuery, GetticketDetailQueryResult>
    {
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetail;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;

        public GetTicketDetailQueryHandler(
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketFeeDetailRepository ticketFeeDetail,
            ITicketCategoryRepository ticketCategoryRepository)
        {
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _ticketFeeDetail = ticketFeeDetail;
            _ticketCategoryRepository = ticketCategoryRepository;
        }

        public GetticketDetailQueryResult Handle(GetticketDetailQuery query)
        {
            var eventTicketDetailDataModel = _eventTicketDetailRepository.GetByEventDetailId(query.EventDetailId);
            var eventTicketDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(eventTicketDetailDataModel);

            var ticketCategoryIdList = eventTicketDetailModel.Select(s => s.TicketCategoryId);
            var ticketCategoryDataModel = _ticketCategoryRepository.GetByEventDetailIds(ticketCategoryIdList);
            var ticketCategoryModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketCategory>>(ticketCategoryDataModel);

            var eventTicketDetailIdList = eventTicketDetailModel.Select(s => s.Id);
            var eventTicketDetailIdDataModel = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetailIdList);
            var eventTicketAttributeModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(eventTicketDetailIdDataModel);

            var eventTicketAttributeIdList = eventTicketAttributeModel.Select(s => s.Id);
            var ticketFeeDetailIdDataModel = _ticketFeeDetail.GetByEventTicketAttributeIds(eventTicketAttributeIdList);
            var ticketFeeDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketFeeDetail>>(ticketFeeDetailIdDataModel);

            return new GetticketDetailQueryResult
            {
                EventTicketDetail = eventTicketDetailModel,
                EventTicketAttribute = eventTicketAttributeModel,
                TicketFeeDetail = ticketFeeDetailModel,
                TicketCategory = ticketCategoryModel,
            };
        }
    }
}