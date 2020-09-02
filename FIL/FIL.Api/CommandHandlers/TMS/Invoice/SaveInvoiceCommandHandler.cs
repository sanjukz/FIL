using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS.Invoice
{
    public class SaveInvoiceCommandHandler : BaseCommandHandlerWithResult<SaveInvoiceCommand, SaveInvoiceCommandResult>
    {
        private readonly ICompanyDetailRepository _companyDetailRepository;
        private readonly IBankDetailRepository _bankDetailRepository;
        private readonly ICorporateInvoiceDetailRepository _corporateInvoiceDetailRepository;
        private readonly ICorporateOrderInvoiceMappingRepository _corporateOrderInvoiceMappingRepository;
        private readonly ICorporateOrderRequestRepository _corporateOrderRequestRepository;
        private readonly Logging.ILogger _logger;

        public SaveInvoiceCommandHandler(Logging.ILogger logger,
        ICompanyDetailRepository companyDetailRepository,
        IBankDetailRepository bankDetailRepository,
        ICorporateInvoiceDetailRepository corporateInvoiceDetailRepository,
        ICorporateOrderInvoiceMappingRepository corporateOrderInvoiceMappingRepository,
        ICorporateOrderRequestRepository corporateOrderRequestRepository,
        IMediator mediator) : base(mediator)
        {
            _companyDetailRepository = companyDetailRepository;
            _bankDetailRepository = bankDetailRepository;
            _corporateInvoiceDetailRepository = corporateInvoiceDetailRepository;
            _corporateOrderInvoiceMappingRepository = corporateOrderInvoiceMappingRepository;
            _corporateOrderRequestRepository = corporateOrderRequestRepository;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(SaveInvoiceCommand command)
        {
            SaveInvoiceCommandResult saveInvoiceCommandResult = new SaveInvoiceCommandResult();
            try
            {
                var corporateInvoiceDetail = new CorporateInvoiceDetail
                {
                    InvoiceDueDate = command.InvoiceDueDate,
                    CompanyDetailId = command.CompanyDetailId,
                    BankDetailId = command.BankDetailId,
                    Address = command.Address,
                    CountryId = command.CountryId,
                    StateId = command.StateId,
                    CityId = command.CityId,
                    CurrencyId = command.CurrencyId,
                    InvoiceDate = DateTime.UtcNow,
                    IsEnabled = true
                };
                CorporateInvoiceDetail corporateInvoiceDetailResult = _corporateInvoiceDetailRepository.Save(corporateInvoiceDetail);
                if (corporateInvoiceDetailResult.Id != 0)
                {
                    foreach (var item in command.CorporateOrderRequestDetails)
                    {
                        var CorporateOrderInvoiceMapping = new CorporateOrderInvoiceMapping
                        {
                            CorporateInvoiceDetailId = corporateInvoiceDetailResult.Id,
                            CorporateOrderRequestId = item.CorporateOrderRequestId,
                            Quantity = item.Quantity,
                            UnitPrice = item.localPrice,
                            TotalTicketAmount = item.Quantity * item.localPrice,
                            ConvenienceCharge = item.ConvenienceCharge,
                            ServiceCharge = item.ServiceCharge,
                            ValueAddedTax = item.ValueAddedTax,
                            DiscountAmount = item.DiscountAmount,
                            NetTicketAmount = (item.Quantity * item.localPrice) + ((item.ConvenienceCharge + item.ServiceCharge) - item.DiscountAmount),
                            IsEnabled = true
                        };
                        _corporateOrderInvoiceMappingRepository.Save(CorporateOrderInvoiceMapping);
                        CorporateOrderRequest corporateOrderRequest = _corporateOrderRequestRepository.Get(item.CorporateOrderRequestId);
                        corporateOrderRequest.OrderStatusId = OrderStatus.InvoiceGenerated;
                        _corporateOrderRequestRepository.Save(corporateOrderRequest);
                    }
                    saveInvoiceCommandResult.Id = corporateInvoiceDetailResult.Id;
                    saveInvoiceCommandResult.Success = true;
                }
                else
                {
                    saveInvoiceCommandResult.Id = -1;
                    saveInvoiceCommandResult.Success = false;
                }
                return Task.FromResult<ICommandResult>(saveInvoiceCommandResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                saveInvoiceCommandResult.Id = -1;
                saveInvoiceCommandResult.Success = false;
                saveInvoiceCommandResult.ErrorMessage = ex.ToString();
                return Task.FromResult<ICommandResult>(saveInvoiceCommandResult);
            }
        }
    }
}