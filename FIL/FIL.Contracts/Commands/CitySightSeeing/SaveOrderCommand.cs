using System;

namespace FIL.Contracts.Commands.CitySightSeeing
{
    public class SaveOrderCommand : BaseCommand
    {
        public long TransactionId { get; set; }
        public string Reservation_reference { get; set; }
        public string Distributor_reference { get; set; }
        public DateTime Reservation_valid_until { get; set; }
        public string FromTime { get; set; }
        public string EndTime { get; set; }
        public string TicketId { get; set; }
        public string TimeSlot { get; set; }
    }
}