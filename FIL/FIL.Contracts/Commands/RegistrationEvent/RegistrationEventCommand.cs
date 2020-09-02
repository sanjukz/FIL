using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.RegistrationEvent
{
    public class RegistrationEventCommand : ICommandWithResult<RegistrationEventCommandResult>
    {
        public string InstaHandle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? Age { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public Guid? CountryId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class RegistrationEventCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public bool IsExisting { get; set; }
        public RegistrationEventUserMapping RegistrationEventUserMapping { get; set; }
    }
}