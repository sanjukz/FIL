namespace FIL.Contracts.QueryResults
{
    public class LoginWithFacebookUserQueryResult
    {
        public bool Success { get; set; }
        public Models.User User { get; set; }
    }
}