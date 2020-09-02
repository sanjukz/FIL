using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class UserProfile
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsRASVMailOPT { get; set; }
        public SignUpMethods? SignUpMethodId { get; set; }
    }
}