using System;

namespace FIL.Contracts.Enums
{
    [Flags]
    public enum PickupType
    {
        None = 0,
        Self = 1,
        Representative,
    }
}