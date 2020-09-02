using System;

namespace FIL.Contracts.Interfaces.Commands
{
    public interface ICommand
    {
        Guid ModifiedBy { get; set; }
    }
}