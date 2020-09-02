using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Tiqets;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Tiqets
{
    public class SyncTiqetProductsCommand : ICommandWithResult<SyncTiqetProductsCommandResult>
    {
        public int SkipIndex { get; set; }
        public int TakeIndex { get; set; }
        public int PageNumber { get; set; }
        public bool GetProducts { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SyncTiqetProductsCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<TiqetProduct> tiqetProducts { get; set; }
        public int? RemainingProducts { get; set; }
    }
}