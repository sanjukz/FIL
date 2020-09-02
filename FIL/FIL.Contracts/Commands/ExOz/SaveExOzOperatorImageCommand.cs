namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzOperatorImageCommand : BaseCommand
    {
        public long? OperatorId { get; set; }
        public string ImageURL { get; set; }
        public string ImageType { get; set; }
        public bool IsEnabled { get; set; }
    }
}