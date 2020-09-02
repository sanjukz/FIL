using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.CorporateRequest
{
    public class SaveCorporateRequestTicketDetailsCommand : ICommandWithResult<SaveCorporateRequestTicketDetailsCommandResult>
    {
        public int CorporateRequestId { get; set; }
        public IEnumerable<CorporateRequestTicketData> CorporateRequestTicketData { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateRequestTicketData
    {
        public long EventTicketAttributeId { get; set; }
        public int Quantity { get; set; }
        public int TicketTypeId { get; set; }
        public decimal TicketPrice { get; set; }
        public bool SeasonPackage { get; set; }
        public decimal seasonPackagePrice { get; set; }
    }

    public class SaveCorporateRequestTicketDetailsCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}