using FIL.Contracts.Models;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.UserCreation
{
    public class ReportingColumnAssignCommand : BaseCommand
    {
        public Guid UserAltId { get; set; }
        public List<ReportingColumn> reportingColumns { get; set; }
    }
}