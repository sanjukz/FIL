using FIL.Api.Repositories;
using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.DataModels;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CitySightSeeing
{
    public class SaveOrderCommandHandler : BaseCommandHandler<SaveOrderCommand>
    {
        private readonly ICitySightSeeingTransactionDetailRepository _citySightSeeingTransactionDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ICitySightSeeingTicketTypeDetailRepository _citySightSeeingTicketTypeDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ILogger _logger;

        public SaveOrderCommandHandler(ICitySightSeeingTransactionDetailRepository citySightSeeingTransactionDetailRepository, ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, ITicketCategoryRepository ticketCategoryRepository, ICitySightSeeingTicketTypeDetailRepository citySightSeeingTicketTypeDetailRepository, IEventTicketDetailRepository eventTicketDetailRepository, ILogger logger,
        IMediator mediator) : base(mediator)
        {
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _citySightSeeingTicketTypeDetailRepository = citySightSeeingTicketTypeDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _logger = logger;
            _citySightSeeingTransactionDetailRepository = citySightSeeingTransactionDetailRepository;
        }

        protected override async Task Handle(SaveOrderCommand command)
        {
            try
            {
                //check if there is discount component available for that variant
                var citySightSeeingTicketTypeDetails = _citySightSeeingTicketTypeDetailRepository.GetAllByTicketId(command.TicketId).Where(s => s.UnitDiscount != "0.00").ToList();
                if (citySightSeeingTicketTypeDetails.Count() > 0)
                {
                    var trasactionDetail = _transactionDetailRepository.GetByTransactionId(command.TransactionId);
                    var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(trasactionDetail.Select(s => s.EventTicketAttributeId));
                    var eventTicketDetails = _eventTicketDetailRepository.GetAllByEventTicketDetailIds(eventTicketAttributes.Select(s => s.EventTicketDetailId));
                    var ticketCategories = _ticketCategoryRepository.GetAllTicketCategory(eventTicketDetails.Select(s => s.TicketCategoryId));
                    decimal totalDiscountAmount = 0;
                    foreach (var currentCitySightSeeingTicketType in citySightSeeingTicketTypeDetails)
                    {
                        var currentTicketCat = ticketCategories.Where(s => s.Name == currentCitySightSeeingTicketType.TicketType).FirstOrDefault();
                        var currentEventTicketDetail = eventTicketDetails.Where(s => s.TicketCategoryId == currentTicketCat.Id).FirstOrDefault();
                        var currentEventTicketAttribute = eventTicketAttributes.Where(s => s.EventTicketDetailId == currentEventTicketDetail.Id).FirstOrDefault();
                        var currentTransactionDetail = trasactionDetail.Where(s => s.EventTicketAttributeId == currentEventTicketAttribute.Id).FirstOrDefault();
                        currentTransactionDetail.DiscountAmount = (Convert.ToInt16(currentCitySightSeeingTicketType.UnitDiscount) * currentTransactionDetail.TotalTickets);
                        _transactionDetailRepository.Save(currentTransactionDetail);
                        totalDiscountAmount = totalDiscountAmount + (Convert.ToInt16(currentCitySightSeeingTicketType.UnitDiscount) * currentTransactionDetail.TotalTickets);
                    }
                    var transaction = _transactionRepository.Get(command.TransactionId);
                    transaction.DiscountAmount = totalDiscountAmount;
                    transaction.NetTicketAmount = transaction.GrossTicketAmount - totalDiscountAmount;
                    _transactionRepository.Save(transaction);
                }
                var citySightSeeingTransactionData = new CitySightSeeingTransactionDetail
                {
                    AltId = Guid.NewGuid(),
                    FromDateTime = command.FromTime,
                    EndDateTime = command.EndTime,
                    HasTimeSlot = true,
                    ReservationDistributorReference = command.Distributor_reference,
                    TicketId = command.TicketId,
                    TransactionId = command.TransactionId,
                    ReservationValidUntil = command.Reservation_valid_until.ToString(),
                    ReservationReference = command.Reservation_reference,
                    TimeSlot = command.TimeSlot,
                    ModifiedBy = command.ModifiedBy
                };
                var citySightSeeingTransaction = _citySightSeeingTransactionDetailRepository.Save(citySightSeeingTransactionData);
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to Disable Event HOHO Data", e));
            }
        }
    }
}