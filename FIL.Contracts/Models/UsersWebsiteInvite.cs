using FIL.Contracts.Interfaces;
using System;

namespace FIL.Contracts.Models
{
    public class UsersWebsiteInvite : IId<long>
    {
        public long Id { get; set; }
        public string UserEmail { get; set; }
        public string UserInviteCode { get; set; }
        public int WebsiteID { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UsedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}