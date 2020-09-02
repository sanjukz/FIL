using System;

namespace FIL.Contracts.Commands.Account
{
    public class SetDefaultAddressCommand : BaseCommand
    {
        public Guid AltId { get; set; }
        public bool MakeDefault { get; set; }
    }
}