namespace FIL.Contracts.Commands._usersWebsiteInvite_Interest
{
    public class UsersWebsiteInvite_InterestCommand : BaseCommand
    {
        public int Nationality { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}