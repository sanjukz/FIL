namespace FIL.Contracts.Commands.RegistrationEvent
{
    public class RegistrationEventSaveTransactionCommand : BaseCommand
    {
        public long TransactionId { get; set; }
        public string RegistrationEventUserId { get; set; }
    }
}