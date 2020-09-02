namespace FIL.Contracts.Commands.FeelNewsLetterSignUp
{
    public class FeelNewsLetterSignUpUserCommand : BaseCommand
    {
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsFeel { get; set; }
    }
}