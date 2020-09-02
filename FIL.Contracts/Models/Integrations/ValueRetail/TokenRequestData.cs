namespace FIL.Contracts.Models.Integrations.ValueRetail
{
    public class TokenRequestData
    {
        private string Resource { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string GrantType { get; set; }
    }
}