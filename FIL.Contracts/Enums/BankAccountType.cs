using FIL.Contracts.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum BankAccountType
    {
        None = 0,

        [Display(Name = "Checking/Current Account")]
        Checking,

        [Display(Name = "Savings Account")]
        Saving,

        [Display(Name = "Salary Account")]
        Salary
    }
}