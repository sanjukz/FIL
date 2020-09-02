using FIL.Contracts.Models.CreateEventV1;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class ReplayDetailCommand : Contracts.Interfaces.Commands.ICommandWithResult<ReplayDetailCommandResult>
    {
        public List<ReplayDetailModel> ReplayDetailModel { get; set; }
        public short CurrentStep { get; set; }
        public long EventId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ReplayDetailCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string CompletedStep { get; set; }
        public short CurrentStep { get; set; }
        public List<ReplayDetailModel> ReplayDetailModel { get; set; }
    }
}