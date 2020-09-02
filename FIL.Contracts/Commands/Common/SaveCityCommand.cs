using System;

namespace FIL.Contracts.Commands.Common
{
    public class SaveCityCommand : BaseCommand
    {
        public string Name { get; set; }
        public Guid StateAltId { get; set; }
    }
}