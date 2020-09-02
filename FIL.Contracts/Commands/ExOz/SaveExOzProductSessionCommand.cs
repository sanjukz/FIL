namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzProductSessionCommand : BaseCommand
    {
        public long ProductSessionId { get; set; }
        public string SessionName { get; set; }
        public long ProductId { get; set; }
        public string HasPickups { get; set; }
        public decimal Levy { get; set; }
        public bool IsExtra { get; set; }
        public string TourHour { get; set; }
        public string TourMinute { get; set; }
        public string TourDuration { get; set; }
        public bool IsEnabled { get; set; }
    }
}