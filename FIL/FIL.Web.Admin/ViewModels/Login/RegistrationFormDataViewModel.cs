using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FIL.Web.Kitms.Feel.ViewModels.Login
{
    public class RegistrationFormDataViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

       
    }



}