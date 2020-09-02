using FIL.Api.Repositories;
using FIL.Contracts.Commands.ContactUsDetail;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ContactUsDetail
{
    public class ContactUsDetailCommandHandler : BaseCommandHandler<ContactUsDetailCommand>
    {
        private readonly IContactUsDetailRepository _contactUsDetailRepository;

        public ContactUsDetailCommandHandler(IContactUsDetailRepository contactUsDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _contactUsDetailRepository = contactUsDetailRepository;
        }

        protected override async Task Handle(ContactUsDetailCommand command)
        {
            var contactUs = new FIL.Contracts.DataModels.ContactUsDetail
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PhoneCode = command.PhoneCode,
                PhoneNumber = command.PhoneNumber,
                Subject = command.Subject,
                Status = command.Status,
                CreatedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                IsEnabled = true
            };

            _contactUsDetailRepository.Save(contactUs);
        }
    }
}