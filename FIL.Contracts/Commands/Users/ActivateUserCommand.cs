using System;

namespace FIL.Contracts.Commands.Users
{
    public class ActivateUserCommand : BaseCommand
    {
        public Guid AltId { get; set; }
    }
}