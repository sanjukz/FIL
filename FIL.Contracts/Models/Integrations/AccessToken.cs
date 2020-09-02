namespace FIL.Contracts.Models.Integrations
{
    public interface IAccessToken
    {
        string Token { get; set; }
        string ExpiresIn { get; set; }
    }

    public class AccessToken : IAccessToken
    {
        public string Token { get; set; }
        public string ExpiresIn { get; set; }
    }
}