namespace FIL.Messaging.Models
{
    public interface IResponse
    {
        bool Success { get; set; }
        string ErrorMessage { get; set; }
    }

    public class Response : IResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}