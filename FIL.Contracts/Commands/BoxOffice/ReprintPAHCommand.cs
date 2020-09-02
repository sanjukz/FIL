using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class ReprintPAHCommand : ICommandWithResult<ReprintPAHCommandResult>
    {
        public long TransactionId { get; set; }
        public Channels ChannelId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ReprintPAHCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public List<MatchSeatTicketDetail> MatchSeatTicketDetail { get; set; }
    }
}