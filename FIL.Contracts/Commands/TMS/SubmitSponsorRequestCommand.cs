using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.TMS;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.TMS
{
    public class SubmitSponsorRequestCommand : ICommandWithResult<SubmitSponsorRequestCommandResult>
    {
        public long SponsorId { get; set; }
        public OrderType OrderTypeId { get; set; }
        public AccountType AccountTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsRepresentative { get; set; }
        public List<SeatDetail> SeatDetails { get; set; }
        public int? RolesId { get; set; }
        public List<SponsorRequestTicketCategory> TicketCategories { get; set; }
        public RepresentativeDetail RepresentativeDetail { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SubmitSponsorRequestCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<SeatDetail> SeatDetails { get; set; }
        public long CorporateTicketAllocationId { get; set; }
    }
}