namespace FIL.Messaging.Models.TextMessages
{
    public interface ITextMessage
    {
        string To { get; set; }
        string From { get; set; }
        string Body { get; set; }
        bool Forced { get; set; }
    }

    public class TextMessage : ITextMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public bool Forced { get; set; }
    }
}