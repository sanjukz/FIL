using FIL.Contracts.Interfaces;
using System;

namespace FIL.Contracts.Models
{
    public class UsersWebsiteInvite_Interest : IId<long>
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public int Nationality { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedUtc { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}