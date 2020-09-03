using FIL.Web.Core.ViewModels;
using Newtonsoft.Json;

namespace FIL.Web.Admin.ViewModels.Login
{
    public class LoginResponseViewModel  : IResponseViewModel
    {
        public bool Success { get; set; }
        public SessionViewModel Session { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsLockedOut { get; set; } = false;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool RequiresTwoFactor { get; set; } = false;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsNotAllowed { get; set; } = false;
    }
}