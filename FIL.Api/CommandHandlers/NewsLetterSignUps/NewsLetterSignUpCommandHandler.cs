using FIL.Api.Repositories;
using FIL.Contracts.Commands.NewsLetterSignUp;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.NewsLetterSignUps
{
    public class NewsLetterSignUpCommandHandler : BaseCommandHandler<NewsLetterSignUpUserCommand>
    {
        private readonly INewsLetterSignUpRepository _newsLetterSignUpRepository;

        public NewsLetterSignUpCommandHandler(INewsLetterSignUpRepository newsLetterSignUpRepository, IMediator mediator)
            : base(mediator)
        {
            _newsLetterSignUpRepository = newsLetterSignUpRepository;
        }

        protected override async Task Handle(NewsLetterSignUpUserCommand command)
        {
            var newsLetterSign = new NewsLetterSignUp
            {
                Email = command.Email,
                CreatedUtc = DateTime.UtcNow,
                CreatedBy = command.ModifiedBy,
                IsEnabled = command.IsEnabled
            };
            _newsLetterSignUpRepository.Save(newsLetterSign);
            return;
        }
    }
}