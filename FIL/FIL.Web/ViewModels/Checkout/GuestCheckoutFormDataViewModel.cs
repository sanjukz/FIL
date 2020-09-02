using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Checkout
{
    public class GuestCheckoutFormDataViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string InviteCode { get; set; }
    }
}
