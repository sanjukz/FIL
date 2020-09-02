namespace FIL.Contracts.Models
{
    public class TournamentLayout
    {
        public int Id { get; set; }
        public string LayoutName { get; set; }
        public long EventId { get; set; }
        public int MasterVenueLayoutId { get; set; }
    }
}