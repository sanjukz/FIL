namespace FIL.Contracts.Commands.Common
{
    public class SaveCountryCommand : BaseCommand
    {
        public string Name { get; set; }
        public string IsoAlphaTwoCode { get; set; }
        public string IsoAlphaThreeCode { get; set; }
    }
}