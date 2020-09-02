using FIL.Contracts.Models.BoxOffice;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class SubmitUserDetailCommand : BaseCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsBOUser { get; set; }
        public Guid? CountryId { get; set; }
        public long? ParentId { get; set; }
        public long RoleId { get; set; }
        public int? UserType { get; set; }
        public bool? IsChildTicket { get; set; }
        public bool? IsSrTicket { get; set; }
        public List<FeeDetailModel> FeeDetail { get; set; }
    }
}