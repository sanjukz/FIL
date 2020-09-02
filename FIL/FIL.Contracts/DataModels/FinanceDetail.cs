using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class FinanceDetail : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public long CurrencyId { get; set; }
        public long CountryId { get; set; }
        public string AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccountType { get; set; }
        public string BankName { get; set; }
        public bool IsBankAccountGST { get; set; }
        public bool IsUpdatLater { get; set; }
        public long StateId { get; set; }
        public string RoutingNo { get; set; }
        public string GSTNo { get; set; }
        public string AccountNo { get; set; }
        public string PANNo { get; set; }
        public string AccountNickName { get; set; }
        public string FinancialsAccountBankAccountGSTInfo { get; set; }
        public long EventId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class FinanceDetailValidator : AbstractValidator<FinanceDetail>, IKzValidator
    {
        public FinanceDetailValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}