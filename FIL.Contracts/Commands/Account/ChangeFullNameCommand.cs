using System;

namespace FIL.Contracts.Commands.Account
{
    public class ChangeFullNameCommand : BaseCommand
    {
        public Guid AltId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isMarketingOptUpdate { get; set; }
        public bool isMarketingOptIn { get; set; }
    }
}