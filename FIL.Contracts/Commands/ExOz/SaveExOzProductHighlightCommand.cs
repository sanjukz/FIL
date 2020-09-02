namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzProductHighlightCommand : BaseCommand
    {
        public long ProductId { get; set; }
        public string Highlight { get; set; }
        public bool IsEnabled { get; set; }
    }
}