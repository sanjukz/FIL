namespace FIL.Contracts.Models
{
    public class CustomerDocumentType
    {
        public long Id { get; set; }
        public string DocumentType { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsASI { get; set; }
    }
}