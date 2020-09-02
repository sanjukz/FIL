using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class PahCommand : ICommandWithResult<PahCommandResult>
    {
        public long TransactionId { get; set; }
        public Guid? TransactionAltId { get; set; }
        public bool isASI { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class PahCommandResult : ICommandResult
    {
        public bool Success { get; set; }
        public long Id { get; set; }
        public IEnumerable<DataModels.MatchSeatTicketInfo> matchSeatTicketInfo { get; set; }
        public IEnumerable<MatchTeamInfo> matchTeamInfo { get; set; }
    }
}