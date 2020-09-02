using FIL.Api.Repositories;
using FIL.Contracts.Queries.TicketCategory;
using FIL.Contracts.QueryResults.TicketCategories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TicketCategory
{
    public class TicketCategoryDetailQueryHandler : IQueryHandler<TicketCategoryDetailQuery, TicketCategoryDetailQueryResult>
    {
        private readonly FIL.Logging.ILogger _logger;
        private readonly ITicketCategoryDetailRepository _ticketCategoryDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;

        public TicketCategoryDetailQueryHandler(
            FIL.Logging.ILogger logger,
            ITicketCategoryRepository ticketCategoryRepository,
            ITicketCategoryDetailRepository ticketCategoryDetailRepository)
        {
            _logger = logger;
            _ticketCategoryDetailRepository = ticketCategoryDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
        }

        public TicketCategoryDetailQueryResult Handle(TicketCategoryDetailQuery query)
        {
            try
            {
                var ticketCategoryDetails = _ticketCategoryDetailRepository.GetAll();
                var ticketCategoryDetailData = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TicketCategoryDetail>>(ticketCategoryDetails);

                var ticketCategoryies = _ticketCategoryRepository.GetAllTicketCategory(ticketCategoryDetails.Select(s => (long)s.TicketCategoryId));
                var ticketCategoryData = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TicketCategory>>(ticketCategoryies);

                return new TicketCategoryDetailQueryResult
                {
                    TicketCategoryDetails = ticketCategoryDetailData,
                    TicketCategories = ticketCategoryData
                };
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return new TicketCategoryDetailQueryResult { };
            }
        }
    }
}