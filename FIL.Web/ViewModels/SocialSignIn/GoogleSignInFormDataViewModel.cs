using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.SocialSignIn
{
    public class GoogleSignInFormDataViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool? OptedForMailer { get; set; }
        public string SocialLoginId { get; set; }
        public SignUpMethods SignUpMethodId { get; set; }
        public string BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public string ResidentId { get; set; }
        public string CitizenId { get; set; }
        public string ReferralId { get; set; }
    }
}
