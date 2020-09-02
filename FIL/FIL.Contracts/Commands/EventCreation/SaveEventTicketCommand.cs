namespace FIL.Contracts.Commands.EventCreation
{
    public class SaveEventTicketCommand : BaseCommand
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}