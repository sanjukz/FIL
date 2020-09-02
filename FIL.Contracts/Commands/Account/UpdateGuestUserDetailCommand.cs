using System;

namespace FIL.Contracts.Commands.Account
{
    public class UpdateGuestUserDetailCommand : BaseCommand
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
    }
}