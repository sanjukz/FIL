namespace FIL.Contracts.Models.Integrations
{
    public interface IResponse<T>
    {
        bool Success { get; set; }
        T Result { get; set; }
    }

    public class Response<T> : IResponse<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
    }
}