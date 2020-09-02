namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IPaymentHtmlPostResponse
    {
        IHtmlPostRequest HtmlPostRequest { get; set; }
        string Error { get; set; }
    }

    public class PaymentHtmlPostResponse : IPaymentHtmlPostResponse
    {
        public IHtmlPostRequest HtmlPostRequest { get; set; }
        public string Error { get; set; }
    }
}