namespace FIL.Contracts.QueryResults.User
{
    public class UserSearchQueryResult
    {
        public bool Success { get; set; }
        public Models.User User { get; set; }
        public string Country { get; set; }
    }
}