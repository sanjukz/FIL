using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.TMS
{
    public class AddCorporateCommand : ICommandWithResult<AddCorporateDataResult>
    {
        public long? Id { get; set; }
        public string SponsorName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public string CityId { get; set; }
        public string StateId { get; set; }
        public string CountryId { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool isEditCompany { get; set; }
        public SponsorType SponsorType { get; set; }
    }

    public class AddCorporateDataResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string message { get; set; }
    }
}