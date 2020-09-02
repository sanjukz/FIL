using System;

namespace FIL.Web.Core.ViewModels
{
    public class UserViewModel
    {
        public Guid AltId { get; set; }
        public long Id { get; set; }
        public int? RolesId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool ProfilePic { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
