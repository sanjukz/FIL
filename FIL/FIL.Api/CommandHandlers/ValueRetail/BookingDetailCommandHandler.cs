using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.ValueRetail;
using FIL.Contracts.DataModels;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;

//using Stripe;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ValueRetail
{
    public class BookingDetailCommandHandler : BaseCommandHandler<BookingDetailCommand>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IMediator _mediator;
        private readonly IValueRetailBookingDetailRepository _valueRetailBookingDetailRepository;

        public BookingDetailCommandHandler(
            ILogger logger,
            ISettings settings,
            IMediator mediator,
            IValueRetailBookingDetailRepository valueRetailBookingDetailRepository
           )
           : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _mediator = mediator;
            _valueRetailBookingDetailRepository = valueRetailBookingDetailRepository;
        }

        protected override async Task Handle(BookingDetailCommand command)
        {
            try
            {
                var VRBooking = _valueRetailBookingDetailRepository.GetByJobId(command.ValueRetailBookingDetail.JobId);
                if (VRBooking == null)
                {
                    VRBooking = _valueRetailBookingDetailRepository.Save(new ValueRetailBookingDetail
                    {
                        AltId = Guid.NewGuid(),
                        JobId = command.ValueRetailBookingDetail.JobId,
                        Email = command.ValueRetailBookingDetail.Email,
                        Date = command.ValueRetailBookingDetail?.Date ?? null,
                        VillageCode = command.ValueRetailBookingDetail.VillageCode,
                        CultureCode = command.ValueRetailBookingDetail.CultureCode,
                        Pricing = (decimal)command.ValueRetailBookingDetail.Pricing,
                        ValueRetailAltId = command.ValueRetailBookingDetail.Id,
                        ReferenceURL = (string)command.ValueRetailBookingDetail.ReferenceURL,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid(),
                        UpdatedBy = null,
                        ModifiedBy = Guid.NewGuid()
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to save Value Retail Villages in Db", ex));
            }
        }
    }
}