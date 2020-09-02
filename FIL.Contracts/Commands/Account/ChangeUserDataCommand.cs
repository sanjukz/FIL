using System;

namespace FIL.Contracts.Commands.Account
{
    public class ChangeUserDataCommand : BaseCommand
    {
        public Guid AltId { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
    }
}