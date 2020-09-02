namespace FIL.Contracts.Commands.Invite
{
    public class UpdateInviteCommand : BaseCommand
    {
        public string UserEmail { get; set; }
        public string UserInviteCode { get; set; }
        public bool IsUsed { get; set; }
        public int Id { get; set; }
    }
}