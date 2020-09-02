using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands
{
    public class BaseCommand : ICommand
    {
        public Guid ModifiedBy { get; set; }
    }
}