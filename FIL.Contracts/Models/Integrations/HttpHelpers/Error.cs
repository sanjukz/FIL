namespace FIL.Contracts.Models.Integrations.HttpHelpers
{
    public interface IError
    {
        string Code { get; set; }
        string Message { get; set; }
    }

    public class Error : IError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}