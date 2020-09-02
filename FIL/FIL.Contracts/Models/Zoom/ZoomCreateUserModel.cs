namespace FIL.Contracts.Models.Zoom
{
    public class UserInfo
    {
        public string email { get; set; }
        public int type { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class ZoomCreateUserModel
    {
        public string action { get; set; }
        public UserInfo user_info { get; set; }
    }
}