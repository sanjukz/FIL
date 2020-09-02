using System;

namespace FIL.Contracts.Commands.Account
{
    public class DeleteGuestUserDetailCommand : BaseCommand
    {
        public Guid UserId { get; set; }
    }
}