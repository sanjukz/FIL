using FIL.Api.PaymentChargers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Payment;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.PaymentChargers;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Payment
{
    public class PaymentResponseCommandHandler : BaseCommandHandlerWithResult<PaymentResponseCommand, PaymentResponseCommandResult>
    {
        private readonly IHdfcEnrolledCharger<IHdfcEnrolledCharge, IPaymentHtmlPostResponse> _hdfcEnrolledCharger;
        private readonly ICcavenueCharger<ICcavenueCharge, IPaymentHtmlPostResponse> _ccavenueCharger;
        private readonly INabTransactCharger<INabTransactCharge, IPaymentHtmlPostResponse> _nabTransactCharger;
        private readonly IPaymentCharger<IStripeCharge, IPaymentResponse> _stripeCharger;
        private readonly ITransactionIdProvider _transactionIdProvider;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly FIL.Logging.ILogger _logger;

        public PaymentResponseCommandHandler(IHdfcEnrolledCharger<IHdfcEnrolledCharge, IPaymentHtmlPostResponse> hdfcEnrolledCharge,
            ICcavenueCharger<ICcavenueCharge, IPaymentHtmlPostResponse> ccavenueCharger,
            INabTransactCharger<INabTransactCharge, IPaymentHtmlPostResponse> nabTransactCharger,
            ITransactionIdProvider transactionIdProvider,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            IPaymentCharger<IStripeCharge, IPaymentResponse> stripeCharger,
            ITransactionRepository transactionRepository,
            FIL.Logging.ILogger logger,
            ICurrencyTypeRepository currencyTypeRepository,
            IMediator mediator)
            : base(mediator)
        {
            _hdfcEnrolledCharger = hdfcEnrolledCharge;
            _ccavenueCharger = ccavenueCharger;
            _nabTransactCharger = nabTransactCharger;
            _transactionIdProvider = transactionIdProvider;
            _transactionRepository = transactionRepository;
            _stripeCharger = stripeCharger;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _logger = logger;
        }

        protected override async Task<ICommandResult> Handle(PaymentResponseCommand command)
        {
            IPaymentResponse paymentResponse = new PaymentResponse();
            try
            {
                var transactionProvider = _transactionIdProvider.Get(command.QueryString);
                if (transactionProvider.TransactionId == 0)
                {
                    return new PaymentResponseCommandResult();
                }

                var paymentGateway = transactionProvider.PaymentGateway;
                TransactionPaymentDetail paymentDetail = null;
                if (!paymentGateway.HasValue)
                {
                    paymentDetail = GetTransactionDetails(transactionProvider.TransactionId);
                    paymentGateway = paymentDetail.PaymentGatewayId;
                }

                var transaction = _transactionRepository.Get(transactionProvider.TransactionId);
                if (paymentGateway == PaymentGateway.Stripe)
                {
                    paymentDetail = paymentDetail ?? GetTransactionDetails(transactionProvider.TransactionId);
                    var currency = _currencyTypeRepository.Get(transaction.CurrencyId);
                    paymentResponse = await _stripeCharger.Charge(new StripeCharge
                    {
                        TransactionId = transactionProvider.TransactionId,
                        Currency = currency.Code,
                        Amount = Convert.ToDecimal(transaction.NetTicketAmount),
                        UserCardDetailId = paymentDetail.UserCardDetailId ?? 0,
                        Token = transactionProvider.Token,
                        ChannelId = command.ChannelId
                    });
                }
                else if (paymentGateway == PaymentGateway.NabTransact)
                {
                    paymentResponse = await _nabTransactCharger.NabTransactResponseHandler(new GatewayCharge
                    {
                        QueryString = command.QueryString
                    });
                }
                else if (paymentGateway == PaymentGateway.HDFC)
                {
                    paymentResponse = _hdfcEnrolledCharger.HdfcResponseHandler(new GatewayCharge
                    {
                        QueryString = command.QueryString,
                        Response = transactionProvider.Response
                    });
                }
                else if (paymentGateway == PaymentGateway.CCAvenue)
                {
                    paymentResponse = _ccavenueCharger.CcavenueResponseHandler(new GatewayCharge
                    {
                        QueryString = command.QueryString
                    });
                }

                return new PaymentResponseCommandResult
                {
                    PaymentResponse = paymentResponse,
                    TransactionAltId = transaction.AltId,
                    Id = transactionProvider.TransactionId
                };
            }
            catch (Exception ex)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, ex);
            }
            return new PaymentResponseCommandResult();
        }

        private TransactionPaymentDetail GetTransactionDetails(long transactionId)
        {
            return _transactionPaymentDetailRepository
                .GetPaymentDetailByTransactionId(transactionId)
                .OrderByDescending(o => o.Id)
                .FirstOrDefault();
        }
    }
}