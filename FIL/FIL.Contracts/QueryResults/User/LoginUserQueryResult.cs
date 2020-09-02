namespace FIL.Contracts.QueryResults
{
    public class LoginUserQueryResult
    {
        public bool Success { get; set; }
        public Models.User User { get; set; }
    }
}