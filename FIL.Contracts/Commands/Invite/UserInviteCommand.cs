namespace FIL.Contracts.Commands.Invite
{
    public class UserInviteCommand : BaseCommand
    {
        public string UserEmail { get; set; }
        public string UserInviteCode { get; set; }
    }
}