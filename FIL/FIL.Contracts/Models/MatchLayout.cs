namespace FIL.Contracts.Models
{
    public class MatchLayout
    {
        public int Id { get; set; }
        public string LayoutName { get; set; }
        public long EventDetailId { get; set; }
        public int TournamentLayoutId { get; set; }
    }
}