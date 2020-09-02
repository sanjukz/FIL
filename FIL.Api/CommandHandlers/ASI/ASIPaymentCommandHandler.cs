using FIL.Api.PaymentChargers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.ASI;
using FIL.Contracts.DataModels;
using FIL.Contracts.DataModels.ASI;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.ASI;
using FIL.Contracts.Models.PaymentChargers;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Payment
{
    public class ASIPaymentCommandHandler : BaseCommandHandlerWithResult<ASIPaymentCommand, ASIPaymentCommandResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly IPaymentCharger<IASITransactCharge, IPaymentResponse> _asiCharge;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly IASIPaymentResponseDetailRepository _aSIPaymentResponseDetailRepository;
        private readonly IASIPaymentResponseDetailTicketMappingRepository _aSIPaymentResponseDetailTicketMappingRepository;
        private readonly FIL.Logging.ILogger _logger;

        public ASIPaymentCommandHandler(
            ITransactionRepository transactionRepository,
            IUserRepository userRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetail,
            IPaymentCharger<IASITransactCharge, IPaymentResponse> asiCharge,
            IUserCardDetailRepository userCardDetailRepository,
            IUserAddressDetailRepository userAddressDetailRepository,
            IASIPaymentResponseDetailRepository aSIPaymentResponseDetailRepository,
            IASIPaymentResponseDetailTicketMappingRepository aSIPaymentResponseDetailTicketMappingRepository,
            FIL.Logging.ILogger logger,
            IMediator mediator
            )
            : base(mediator)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _userAddressDetailRepository = userAddressDetailRepository;
            _userRepository = userRepository;
            _userCardDetailRepository = userCardDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetail;
            _aSIPaymentResponseDetailRepository = aSIPaymentResponseDetailRepository;
            _aSIPaymentResponseDetailTicketMappingRepository = aSIPaymentResponseDetailTicketMappingRepository;
            _asiCharge = asiCharge;
            _logger = logger;
        }

        private void savePaymentResponse(ASIResponseFormData query) // Save ASI POST Response Data
        {
            try
            {
                var asiPaymentResponse = new ASIPaymentResponseDetail
                {
                    Message = query.Message,
                    IsSuccess = query.IsSuccess,
                    PaymentAmount = query.Data != null ? query.Data.Payment.Amount : 0,
                    PaymentGateway = query.Data != null ? query.Data.Payment.Gateway : "",
                    PaymentId = query.Data != null ? query.Data.Payment.Id : "",
                    PaymentProvider = query.Data != null ? query.Data.Payment.Provider : "",
                    PaymentStatus = query.Data != null ? query.Data.Payment.Status : "",
                    PaymentTimeStamp = query.Data != null ? query.Data.Payment.Date : DateTime.UtcNow,
                    PaymentTransactionId = query.Data != null ? query.Data.Payment.TransactionId : "",
                    TransactionId = query.Data != null ? query.Data.TransactionId : query.TransactionId,
                    Error = query.Error != null ? query.Error : "",
                    IsEnabled = true,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                };
                var savedasiPaymentResponse = _aSIPaymentResponseDetailRepository.Save(asiPaymentResponse);
                if (asiPaymentResponse.IsSuccess)
                {
                    foreach (Ticket currentTicket in query.Data.Tickets)
                    {
                        var asiTicket = new FIL.Contracts.DataModels.ASI.ASIPaymentResponseDetailTicketMapping
                        {
                            Age = currentTicket.Age,
                            Amount = currentTicket.Amount,
                            ASIPaymentResponseDetailId = savedasiPaymentResponse.Id,
                            IsAdult = currentTicket.IsAdult,
                            MonumentTimeSlotId = currentTicket.Monument.Timeslot.Id,
                            Date = DateTime.UtcNow,
                            Gender = currentTicket.Gender,
                            IdentityNo = currentTicket.Identity.No,
                            IsOptional = currentTicket.Monument.Optional,
                            IdentityType = currentTicket.Identity.Type,
                            MonumentCode = currentTicket.Monument.Code,
                            MonumentName = currentTicket.Monument.Name,
                            Name = currentTicket.Name,
                            NationalityCountry = currentTicket.Nationality.Country,
                            NationalityGroup = currentTicket.Nationality.Group,
                            QrCode = currentTicket.QrCode,
                            TicketNo = currentTicket.No,
                            VisitorId = currentTicket.VisitorId,
                            IsEnabled = false,
                            CreatedUtc = DateTime.UtcNow,
                            UpdatedUtc = DateTime.UtcNow
                        };
                        _aSIPaymentResponseDetailTicketMappingRepository.Save(asiTicket);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
        }

        protected override async Task<ICommandResult> Handle(ASIPaymentCommand query)
        {
            var transaction = new Transaction();
            try
            {
                try
                {
                    if (query.IsSuccess && query.aSIResponseFormData.Data != null)
                    {
                        string responseData = JsonConvert.SerializeObject(query.aSIResponseFormData);
                        _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                        {
                            TransactionId = Convert.ToInt64(query.aSIResponseFormData.Data.TransactionId),
                            PaymentOptionId = PaymentOptions.None,
                            PaymentGatewayId = PaymentGateway.Payu,
                            RequestType = "Response",
                            Amount = query.aSIResponseFormData.Data.Payment.Amount.ToString(),
                            PayConfNumber = query.aSIResponseFormData.Data.Payment.Id,
                            PaymentDetail = responseData,
                        });
                    }
                }
                catch (Exception e)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, e);
                }
                if (query.IsSuccess && query.aSIResponseFormData.Data != null)
                {
                    transaction = _transactionRepository.Get(Convert.ToInt64(query.aSIResponseFormData.Data.TransactionId)); // Our Transaction Id return by them... Check if transaction exists for that transactionId
                    savePaymentResponse(query.aSIResponseFormData);
                    User user = _userRepository.GetByEmail(transaction.EmailId);
                    if (user != null)
                    {
                        var addressDetail = new UserAddressDetail
                        {
                            UserId = user.Id,
                            AltId = Guid.NewGuid(),
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            PhoneCode = user.PhoneCode,
                            PhoneNumber = user.PhoneNumber,
                            AddressLine1 = "Zoonga",
                            Zipcode = 1,
                            AddressTypeId = AddressTypes.Billing,
                            IsEnabled = true
                        };
                        _userAddressDetailRepository.Save(addressDetail);
                    }
                    var paymentResponse = await _asiCharge.Charge(new ASITransactCharge
                    {
                        TransactionId = transaction.Id,
                        IsSuccess = query.IsSuccess,
                        asiFormData = query.aSIResponseFormData
                    });
                    if (paymentResponse.Success) // success
                    {
                        return new ASIPaymentCommandResult
                        {
                            Id = transaction != null ? transaction.Id : 0,
                            TransactionId = transaction != null ? transaction.Id : 0,
                            Transaction = transaction,
                            TransactionAltId = transaction.AltId,
                            PaymentResponse = paymentResponse
                        };
                    }
                    else // failure from zoonga
                    {
                        if (query.aSIResponseFormData.TransactionId != null)
                        {
                            var data = query.aSIResponseFormData.TransactionId.Split("-");
                            if (data.Count() > 0)
                            {
                                transaction = _transactionRepository.Get(Convert.ToInt32(data[1]));
                            }
                        }
                        return new ASIPaymentCommandResult
                        {
                            TransactionId = transaction != null ? transaction.Id : 0,
                            Transaction = transaction,
                            PaymentResponse = new PaymentResponse
                            {
                                Success = false,
                                PaymentGatewayError = PaymentGatewayError.Unknown,
                            }
                        };
                    }
                }
                else  // failure from ASI
                {
                    if (query.aSIResponseFormData != null && query.aSIResponseFormData.TransactionId != null)
                    {
                        var data = query.aSIResponseFormData.TransactionId.Split("-");
                        if (data.Count() > 0)
                        {
                            transaction = _transactionRepository.Get(Convert.ToInt32(data[1]));
                        }
                    }
                    savePaymentResponse(query.aSIResponseFormData);
                    return new ASIPaymentCommandResult
                    {
                        TransactionId = transaction != null ? transaction.Id : 0,
                        Transaction = transaction,
                        PaymentResponse = new PaymentResponse
                        {
                            Success = false,
                            PaymentGatewayError = PaymentGatewayError.Unknown
                        }
                    };
                }
            }
            catch (Exception ex) // catch
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new ASIPaymentCommandResult
                {
                    TransactionId = transaction != null ? transaction.Id : 0,
                    Transaction = transaction,
                    PaymentResponse = new PaymentResponse
                    {
                        Success = false,
                        PaymentGatewayError = PaymentGatewayError.Unknown
                    }
                };
            }
        }
    }
}