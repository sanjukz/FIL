using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum TicketLimitCheckTypes
    {
        None = 0,
        Phone = 1,
        Email = 2
    }
}