
namespace FIL.Web.Core.ViewModels.Login
{
    public class LoginWithOTPResponseModel
    {
        public bool Success { get; set; }
        public bool? IsAdditionalFieldsReqd { get; set; } //for new user  
        public bool? EmailAlreadyRegistered { get; set; }
        public Contracts.Models.User User { get; set; }
        public SessionViewModel Session { get; set; }
    }
}
