using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FIL.Web.Feel.ViewModels.Login
{
    public class RegistrationFormDataViewModel : Core.ViewModels.Register.RegisterFormDataViewModel
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ReturnUrl { get; set; }
    }
}
