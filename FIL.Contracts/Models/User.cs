using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class User
    {
        public Guid AltId { get; set; }
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RolesId { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public Channels ChannelId { get; set; }
        public bool LockOutEnabled { get; set; }
        public bool profilePic { get; set; }
        public bool IsActivated { get; set; }
        public bool IsFeelExists { get; set; }
    }
}