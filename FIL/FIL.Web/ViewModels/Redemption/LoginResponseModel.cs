using FIL.Contracts.Models;

namespace FIL.Web.Feel.ViewModels.Redemption
{
    public class LoginResponseModel
    {
        public Contracts.Models.User User { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsPasswordValid { get; set; }
    }
}
