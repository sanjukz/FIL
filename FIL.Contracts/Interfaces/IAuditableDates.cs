using System;

namespace FIL.Contracts.Interfaces
{
    public interface IAuditableDates
    {
        DateTime CreatedUtc { get; set; }
        DateTime? UpdatedUtc { get; set; }
    }
}