using System;

namespace FIL.Contracts.Commands.Common
{
    public class SaveZipcodeCommand : BaseCommand
    {
        public string Zipcode { get; set; }
        public string Region { get; set; }
        public Guid CityAltId { get; set; }
    }
}