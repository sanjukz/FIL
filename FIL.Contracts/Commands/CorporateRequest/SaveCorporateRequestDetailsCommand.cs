using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.CorporateRequest
{
    public class SaveCorporateRequestDetailsCommand : ICommandWithResult<SaveCorporateRequestDetailsCommandResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string Email { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveCorporateRequestDetailsCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public int Otp { get; set; }
        public FIL.Contracts.DataModels.CorporateRequest CorporateRequest { get; set; }
    }
}