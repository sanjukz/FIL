namespace FIL.Contracts.QueryResults.FeelRedemption
{
    public class UserLoginUserQueryResult
    {
        public Models.User User { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsPasswordValid { get; set; }
    }
}