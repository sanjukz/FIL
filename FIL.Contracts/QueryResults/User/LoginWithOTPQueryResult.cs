namespace FIL.Contracts.QueryResults
{
    public class LoginWithOTPQueryResult
    {
        public bool Success { get; set; }
        public bool? IsAdditionalFieldsReqd { get; set; } //for new user
        public Models.User User { get; set; }
    }
}