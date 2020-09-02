namespace FIL.Contracts.Commands.UserCreation
{
    public class UserCreationCommand : BaseCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsVendor { get; set; }
    }
}