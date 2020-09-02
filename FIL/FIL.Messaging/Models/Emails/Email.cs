using System.Collections.Generic;

namespace FIL.Messaging.Models.Emails
{
    public interface IEmail
    {
        string To { get; set; }
        string Bcc { get; set; }
        string From { get; set; }
        string MailBody { get; set; }
        string Subject { get; set; }
        string ConfigurationSetName { get; set; }
        string TemplateName { get; set; }
        bool IsAttachment { get; set; }

        //Stream stream { get; set; }
        byte[] pdfdata { get; set; }

        IDictionary<string, object> Variables { get; set; }
    }

    public class Email : IEmail
    {
        public string To { get; set; }
        public string Bcc { get; set; }
        public string From { get; set; }
        public string TemplateName { get; set; }
        public string MailBody { get; set; }
        public string Subject { get; set; }
        public bool IsAttachment { get; set; }
        public string ConfigurationSetName { get; set; }

        //public Stream stream { get; set; }
        public byte[] pdfdata { get; set; }

        public IDictionary<string, object> Variables { get; set; }
    }
}