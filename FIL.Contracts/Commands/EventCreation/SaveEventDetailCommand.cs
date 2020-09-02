using System;
using System.ComponentModel.DataAnnotations;

namespace FIL.Contracts.Commands.EventCreation
{
    public class SaveEventDetailCommand : Contracts.Interfaces.Commands.ICommandWithResult<SaveEventDetailDataResult>
    {
        public long Id { get; set; }

        [Required]
        public long EventId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int VenueId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public string MetaDetails { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        [Required]
        public int GroupId { get; set; }

        public Guid ModifiedBy { get; set; }
    }

    public class SaveEventDetailDataResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public int VenueId { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}