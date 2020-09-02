using System;

namespace FIL.Contracts.Commands.Account
{
    public class ChangeProfilePicCommand : BaseCommand
    {
        public Guid AltId { get; set; }
        public bool ProfilePic { get; set; }
    }
}