using FIL.Contracts.Models.Integrations.ValueRetail;

namespace FIL.Contracts.Commands.ValueRetail
{
    public class BookingDetailCommand : BaseCommand
    {
        public BookCartResponse ValueRetailBookingDetail { get; set; }
    }
}