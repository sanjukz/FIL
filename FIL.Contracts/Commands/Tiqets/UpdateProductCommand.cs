using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Tiqets
{
    public class UpdateProductCommand : ICommandWithResult<UpdateProductCommandResult>
    {
        public Guid ModifiedBy { get; set; }
        public string productId { get; set; }
        public bool IsImageUpload { get; set; }
    }

    public class UpdateProductCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool success { get; set; }
        public List<string> ImageLinks { get; set; }
        public bool IsImageUpload { get; set; }
        public Guid EventAltId { get; set; }
    }
}