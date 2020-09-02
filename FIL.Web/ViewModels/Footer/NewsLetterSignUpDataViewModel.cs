using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using FIL.Contracts.Enums;

namespace FIL.Web.feel.ViewModels.Footer
{
    public class NewsLetterSignUpDataViewModel
    {
        [Required]
        public string Email { get; set; }
        public bool IsFeel { get; set; }
    }
}
