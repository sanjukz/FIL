using Mandrill.Models;

namespace FIL.MailChimp.Models
{
    public class MCUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SignUpType { get; set; }
        public bool IsCreator { get; set; }
    }

    public class MCUserAdditionalDetailModel: MCUserModel
    {
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string LastEventName { get; set; }
        public string LastPurchaseLocation { get; set; }
        public string LastPurchaseChannel { get; set; }
        public string LastEventTicketCategory { get; set; }
        public string LastEventType { get; set; }
        public string LastEventTicketType { get; set; }
        public string LastEventSubcategory { get; set; }
        public string LastPurchaseAmount { get; set; }
        public string LastPurchaseDate { get; set; }
        public string LastEventCategory { get; set; }
        public string LastEventLocation { get; set; }
    }
}
