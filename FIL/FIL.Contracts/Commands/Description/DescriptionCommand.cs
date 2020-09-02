using System;

namespace FIL.Contracts.Commands.Description
{
    public class DescriptionCommand : Contracts.Interfaces.Commands.ICommandWithResult<DescriptionCommandResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsCountryDescription { get; set; }
        public FIL.Contracts.DataModels.City City { get; set; }
        public FIL.Contracts.DataModels.Country Country { get; set; }
        public bool IsStateDescription { get; set; }
        public bool IsCityDescription { get; set; }
        public int StateId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class DescriptionCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}