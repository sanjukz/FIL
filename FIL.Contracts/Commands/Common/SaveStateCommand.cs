using System;

namespace FIL.Contracts.Commands.Common
{
    public class SaveStateCommand : BaseCommand
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public Guid CountryAltId { get; set; }
    }
}