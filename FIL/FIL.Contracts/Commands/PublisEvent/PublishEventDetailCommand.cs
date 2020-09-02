using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIL.Contracts.Commands.PublishEvent
{
    public class PublishEventDetailCommand : BaseCommand
    {
        [Required]
        public FIL.Contracts.Models.Event Event { get; set; }

        [Required]
        public List<FIL.Contracts.Models.EventDetail> SubEvents { get; set; }

        [Required]
        public bool isHotTicket { get; set; }

        [Required]
        public short SortOrder { get; set; }

        [Required]
        public FIL.Contracts.Enums.Site Site { get; set; }
    }
}