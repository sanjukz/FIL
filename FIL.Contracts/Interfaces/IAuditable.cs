using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.Interfaces
{
    public interface IAuditable : IAuditableDates
    {
        Guid CreatedBy { get; set; }
        Guid? UpdatedBy { get; set; }

        [NotMapped]
        Guid ModifiedBy { get; set; }
    }
}