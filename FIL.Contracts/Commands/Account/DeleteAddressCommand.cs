using System;

namespace FIL.Contracts.Commands.Account
{
    public class DeleteAddressCommand : BaseCommand
    {
        public Guid AltId { get; set; }
    }
}