using FIL.Api.Repositories;
using FIL.Contracts.Queries.EventCreation;
using FIL.Contracts.QueryResults.EventCreation;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.EventCreation
{
    public class EventTicketcategoryQueryHandler : IQueryHandler<EventTicketCreationQuery, EventTicketCreationTicketQueryResult>
    {
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly FIL.Logging.ILogger _logger;

        public EventTicketcategoryQueryHandler(ITicketCategoryRepository ticketCategoryRepository, FIL.Logging.ILogger logger)
        {
            _ticketCategoryRepository = ticketCategoryRepository;
            _logger = logger;
        }

        public EventTicketCreationTicketQueryResult Handle(EventTicketCreationQuery query)
        {
            List<FIL.Contracts.Models.TicketCategory> ticketCategories = new List<FIL.Contracts.Models.TicketCategory>();
            try
            {
                var ticketCategory = _ticketCategoryRepository.GetAll(null);
                foreach (var item in ticketCategory)
                {
                    ticketCategories.Add(new FIL.Contracts.Models.TicketCategory
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
                }
                return new EventTicketCreationTicketQueryResult
                {
                    TicektCategories = ticketCategories
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new EventTicketCreationTicketQueryResult
                {
                    TicektCategories = null
                };
            }
        }
    }
}