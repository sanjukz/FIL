using System.Collections.Generic;

namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IHtmlPostRequest
    {
        string Method { get; set; }
        string Action { get; set; }
        Dictionary<string, string> FormFields { get; set; }
    }

    public class HtmlPostRequest : IHtmlPostRequest
    {
        public string Method { get; set; }
        public string Action { get; set; }
        public Dictionary<string, string> FormFields { get; set; }
    }
}