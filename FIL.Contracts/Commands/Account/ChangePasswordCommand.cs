using System;

namespace FIL.Contracts.Commands.Account
{
    public class ChangePasswordCommand : BaseCommand
    {
        public Guid AltId { get; set; }
        public string PasswordHash { get; set; }
    }
}