namespace FIL.Contracts.Commands.NewsLetterSignUp
{
    public class NewsLetterSignUpUserCommand : BaseCommand
    {
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
    }
}