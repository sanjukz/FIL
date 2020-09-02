namespace FIL.Contracts.Commands.ContactUsDetail
{
    public class ContactUsDetailCommand : BaseCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
    }
}