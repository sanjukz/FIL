using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class ReprintRequestDataContainer
    {
        public IEnumerable<ReprintRequestDetail> ReprintRequestDetail
        { get; set; }

        public IEnumerable<ReprintRequest> ReprintRequest
        { get; set; }
    }
}