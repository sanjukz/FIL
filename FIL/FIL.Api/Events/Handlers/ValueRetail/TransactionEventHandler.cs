using FIL.Api.Events.Event.ValueRetail;
using FIL.Api.Integrations.HttpHelpers;
using FIL.Api.Integrations.ValueRetail;
using FIL.Contracts.Models.Integrations.ValueRetail;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FIL.Api.Events.Handlers.ValueRetail
{
    public class TransactionEventHandler : INotificationHandler<TransactionEvent>
    {
        private IBooking _booking;
        private readonly FIL.Logging.ILogger _logger;

        public TransactionEventHandler(IBooking booking)
        {
            _booking = booking;
        }

        public Task Handle(TransactionEvent notification, CancellationToken cancellationToken)
        {
            // Needs to check its a VR transaction then, Call the confirm function of VR
            var bookingData = new BookCartRequest();
            var bookingResponse = _booking.BookCart(bookingData);
            BookCartResponse bookCartResponse = Mapper<BookCartResponse>.MapJsonStringToObject(bookingResponse.Result.Result);
            return Task.FromResult(0);
        }
    }
}