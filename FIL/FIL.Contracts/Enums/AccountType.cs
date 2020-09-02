using FIL.Contracts.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum AccountType
    {
        None = 0,

        [Display(Name = "Individual")]
        Individual,

        [Display(Name = "Company")]
        Company
    }
}