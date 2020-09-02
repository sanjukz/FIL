using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.Kidzania
{
    public class ShiftResponse
    {
        public List<ShiftOut> ShiftOut { get; set; }
        public List<ParkOut> ParkOut { get; set; }
    }

    public class ShiftOut
    {
        public long CityId { get; set; }
        public long ParkId { get; set; }
        public string ParkName { get; set; }
        public long ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string StartOur { get; set; }
        public string EndOur { get; set; }
        public int ErrId { get; set; }
        public string ErrorString { get; set; }
    }

    public class ParkOut
    {
        public long CityId { get; set; }
        public long ParkId { get; set; }
        public long ShiftId { get; set; }
        public int ErrId { get; set; }
        public string ErrorString { get; set; }
        public string Status { get; set; }
    }
}