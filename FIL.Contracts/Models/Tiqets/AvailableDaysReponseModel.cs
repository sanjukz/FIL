using System.Collections.Generic;

namespace FIL.Contracts.Models.Tiqets
{
    public class AvailableDaysReponseModel
    {
        public bool success { get; set; }
        public List<string> days { get; set; }
    }
}