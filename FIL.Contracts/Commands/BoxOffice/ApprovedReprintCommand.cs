using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class ApprovedReprintCommand : BaseCommand

    {
        public List<long> ReprintRequestDetailId { get; set; }
    }
}