namespace FIL.Contracts.Commands.Account
{
    public class SaveGuestUserDetailCommand : BaseCommand
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
    }
}