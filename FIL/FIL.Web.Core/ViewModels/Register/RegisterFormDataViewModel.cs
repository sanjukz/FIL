using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FIL.Web.Core.ViewModels.Register
{
    public class RegisterFormDataViewModel
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
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public Site? SiteId { get; set; }
        public string BirthDate { get; set; }
        public Guid? ResidentOf { get; set; }
        public Guid? CitizenOf { get; set; }
        public string InviteCode { get; set; }
        public bool? IsMailOpt { get; set; }
        public string ReferralId { get; set; }
    }
}
