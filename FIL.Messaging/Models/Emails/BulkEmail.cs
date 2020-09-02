using System.Collections.Generic;

namespace FIL.Messaging.Models.Emails
{
    public interface IBulkEmail
    {
        /// <summary>
        /// Overrides the individual email object TemplateNames
        /// </summary>
        string TemplateName { get; set; }

        IList<IEmail> Emails { get; set; }
    }

    public class BulkEmail : IBulkEmail
    {
        public string TemplateName { get; set; }
        public IList<IEmail> Emails { get; set; }
    }
}