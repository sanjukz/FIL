using FIL.Contracts.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum ApproveStatus
    {
        None = 0,

        [Display(Name = "Blocked")]
        Blocked,

        [Display(Name = "Pending")]
        Pending,

        [Display(Name = "Approved")]
        Approved,

        [Display(Name = "Success")]
        Success
    }
}