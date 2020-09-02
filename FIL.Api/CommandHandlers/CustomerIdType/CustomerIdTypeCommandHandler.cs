using FIL.Api.Repositories;
using FIL.Contracts.Commands.CustomerIdType;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CustomerIdType
{
    public class CustomerIdTypeCommandHandler : BaseCommandHandler<CustomerIdTypeCommand>
    {
        private readonly ICustomerDocumentTypeRepository _customerDocumentTypeRepository;
        private readonly IMediator _mediator;

        public CustomerIdTypeCommandHandler(
            ICustomerDocumentTypeRepository customerDocumentTypeRepository,
            IMediator mediator)
            : base(mediator)
        {
            _customerDocumentTypeRepository = customerDocumentTypeRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(CustomerIdTypeCommand command)
        {
            FIL.Contracts.DataModels.CustomerDocumentType customerDocumentType = new CustomerDocumentType();

            try
            {
                customerDocumentType.DocumentType = command.CustomerIdType;
                customerDocumentType.IsEnabled = false;
                customerDocumentType.CreatedUtc = DateTime.UtcNow;
                _customerDocumentTypeRepository.Save(customerDocumentType);
            }
            catch (Exception e)
            {
            }
        }
    }
}