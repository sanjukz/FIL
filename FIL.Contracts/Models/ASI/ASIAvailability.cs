namespace FIL.Contracts.Models.ASI
{
    public class ASIAvailability
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Periodicity { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public long MonumentId { get; set; }
        public long Tickets { get; set; }
    }
}