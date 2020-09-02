using System;

namespace FIL.Contracts.Commands.Account
{
    public class ChangePrimaryPhoneCommand : BaseCommand
    {
        public Guid AltId { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}