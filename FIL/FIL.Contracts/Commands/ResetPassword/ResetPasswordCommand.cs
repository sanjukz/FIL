using System;

namespace FIL.Contracts.Commands.ResetPassword
{
    public class ResetPasswordCommand : BaseCommand
    {
        public Guid AltId { get; set; }
        public string PasswordHash { get; set; }
    }
}