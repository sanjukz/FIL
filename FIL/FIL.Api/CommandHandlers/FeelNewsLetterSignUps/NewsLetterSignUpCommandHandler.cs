using FIL.Api.Repositories;
using FIL.Contracts.Commands.FeelNewsLetterSignUp;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.FeelNewsLetterSignUps
{
    public class FeelNewsLetterSignUpCommandHandler : BaseCommandHandler<FeelNewsLetterSignUpUserCommand>
    {
        private readonly INewsLetterSignUpRepository _newsLetterSignUpRepository;

        public FeelNewsLetterSignUpCommandHandler(INewsLetterSignUpRepository newsLetterSignUpRepository, IMediator mediator)
            : base(mediator)
        {
            _newsLetterSignUpRepository = newsLetterSignUpRepository;
        }

        protected override async Task Handle(FeelNewsLetterSignUpUserCommand command)
        {
            var newsLetterSign = new NewsLetterSignUp
            {
                Email = command.Email,
                CreatedUtc = DateTime.UtcNow,
                CreatedBy = command.ModifiedBy,
                IsEnabled = command.IsEnabled,
                IsFeel = command.IsFeel,
            };
            _newsLetterSignUpRepository.Save(newsLetterSign);
            return;
        }
    }
}