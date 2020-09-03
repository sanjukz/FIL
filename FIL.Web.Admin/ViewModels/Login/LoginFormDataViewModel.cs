using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FIL.Web.Admin.ViewModels.Login
{
    public class LoginFormDataViewModel : Core.ViewModels.Login.LoginFormDataViewModel
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ReturnUrl { get; set; }
    }
}