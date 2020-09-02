namespace FIL.Contracts.QueryResults
{
    public class LoginWithGoogleUserQueryResult
    {
        public bool Success { get; set; }
        public Models.User User { get; set; }
    }
}