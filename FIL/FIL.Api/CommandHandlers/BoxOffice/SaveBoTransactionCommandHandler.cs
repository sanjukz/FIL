using FIL.Api.Integrations;
using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class SaveBoTransactionCommandHandler : BaseCommandHandlerWithResult<SaveTransactionCommand, SaveTransactionCommandResult>
    {
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IBoCustomerDetailRepository _boCustomerDetailRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITransactionSeatDetailRepository _transactionSeatDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IIPDetailRepository _iPDetailRepository;
        private readonly IIpApi _ipApi;
        private readonly Logging.ILogger _logger;
        private readonly IWheelchairSeatMappingRepository _wheelchairSeatMappingRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ISeatBlockingProvider _seatBlockingProvider;
        private readonly ITransactionStatusUpdater _transactionStatusUpdater;

        public SaveBoTransactionCommandHandler(
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICountryRepository countryRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IBoCustomerDetailRepository boCustomerDetailRepository,
            ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            ITransactionSeatDetailRepository transactionSeatDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventDetailRepository eventDetailRepository, IIPDetailRepository iPDetailRepository, IIpApi ipApi,
            Logging.ILogger logger,
            IWheelchairSeatMappingRepository wheelchairSeatMappingRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ISeatBlockingProvider seatBlockingProvider,
            ITransactionStatusUpdater transactionStatusUpdater,
        IMediator mediator) : base(mediator)
        {
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _countryRepository = countryRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _boCustomerDetailRepository = boCustomerDetailRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _transactionSeatDetailRepository = transactionSeatDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _iPDetailRepository = iPDetailRepository;
            _ipApi = ipApi;
            _logger = logger;
            _wheelchairSeatMappingRepository = wheelchairSeatMappingRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _seatBlockingProvider = seatBlockingProvider;
            _transactionStatusUpdater = transactionStatusUpdater;
        }

        protected override Task<ICommandResult> Handle(SaveTransactionCommand command)
        {
            SaveTransactionCommandResult saveTransactionCommandResult = new SaveTransactionCommandResult();
            List<TransactionDetail> transactionDetailList = new List<TransactionDetail>();
            FIL.Contracts.DataModels.Transaction transaction = new FIL.Contracts.DataModels.Transaction();
            var eventTicketAttributeIds = command.EventTicketAttributeList.Select(s => s.Id).Distinct();
            var eventTicketAttributes = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(_eventTicketAttributeRepository.GetByIds(eventTicketAttributeIds));
            try
            {
                decimal grossTicketAmount = 0, totalDiscountAmount = 0, netTicketAmount = 0;
                foreach (var ticketAttributes in command.EventTicketAttributeList)
                {
                    var eventTicketAttribute = eventTicketAttributes.FirstOrDefault(w => w.Id == ticketAttributes.Id);
                    if (ticketAttributes.TicketTypeId == TicketType.Child)
                    {
                        if (Convert.ToInt16(ticketAttributes.TotalTickets) <= eventTicketAttribute.RemainingTicketForSale && Convert.ToInt16(ticketAttributes.TotalTickets) <= eventTicketAttribute.ChildQTY)
                        {
                            TransactionDetail transactionDetail = new TransactionDetail();
                            transactionDetail.EventTicketAttributeId = ticketAttributes.Id;
                            transactionDetail.TotalTickets = Convert.ToInt16(ticketAttributes.TotalTickets);
                            transactionDetail.PricePerTicket = ticketAttributes.Price;
                            transactionDetail.DiscountAmount = ticketAttributes.discountedPrice;
                            transactionDetail.TicketTypeId = (short)(TicketType)ticketAttributes.TicketTypeId;
                            netTicketAmount += ((ticketAttributes.TotalTickets * ticketAttributes.Price) - (ticketAttributes.discountedPrice));
                            grossTicketAmount += ((ticketAttributes.TotalTickets * ticketAttributes.Price));
                            totalDiscountAmount += ticketAttributes.discountedPrice;
                            transactionDetailList.Add(transactionDetail);
                        }
                        else
                        {
                            saveTransactionCommandResult.Id = -1;
                            saveTransactionCommandResult.Success = false;
                            saveTransactionCommandResult.ErrorMessage = "Ticket quntity is not avilable ";
                            return Task.FromResult<ICommandResult>(saveTransactionCommandResult);
                        }
                    }
                    else if (ticketAttributes.TicketTypeId == TicketType.SeniorCitizen)
                    {
                        if (Convert.ToInt16(ticketAttributes.TotalTickets) <= eventTicketAttribute.RemainingTicketForSale && Convert.ToInt16(ticketAttributes.TotalTickets) <= eventTicketAttribute.SRCitizenQTY)
                        {
                            TransactionDetail transactionDetail = new TransactionDetail();
                            transactionDetail.EventTicketAttributeId = ticketAttributes.Id;
                            transactionDetail.TotalTickets = Convert.ToInt16(ticketAttributes.TotalTickets);
                            transactionDetail.PricePerTicket = ticketAttributes.Price;
                            transactionDetail.DiscountAmount = ticketAttributes.discountedPrice;
                            transactionDetail.TicketTypeId = (short)(TicketType)ticketAttributes.TicketTypeId;
                            netTicketAmount += ((ticketAttributes.TotalTickets * ticketAttributes.Price) - (ticketAttributes.discountedPrice));
                            grossTicketAmount += ((ticketAttributes.TotalTickets * ticketAttributes.Price));
                            totalDiscountAmount += ticketAttributes.discountedPrice;
                            transactionDetailList.Add(transactionDetail);
                        }
                        else
                        {
                            saveTransactionCommandResult.Id = -1;
                            saveTransactionCommandResult.Success = false;
                            saveTransactionCommandResult.ErrorMessage = "Ticket quantity not available";
                            return Task.FromResult<ICommandResult>(saveTransactionCommandResult);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt16(ticketAttributes.TotalTickets) <= eventTicketAttribute.RemainingTicketForSale)
                        {
                            TransactionDetail transactionDetail = new TransactionDetail();
                            transactionDetail.EventTicketAttributeId = ticketAttributes.Id;
                            transactionDetail.TotalTickets = Convert.ToInt16(ticketAttributes.TotalTickets);
                            transactionDetail.PricePerTicket = ticketAttributes.Price;
                            transactionDetail.DiscountAmount = ticketAttributes.discountedPrice;
                            transactionDetail.TicketTypeId = (short)(TicketType)ticketAttributes.TicketTypeId;
                            netTicketAmount += ((ticketAttributes.TotalTickets * ticketAttributes.Price) - (ticketAttributes.discountedPrice));
                            grossTicketAmount += ((ticketAttributes.TotalTickets * ticketAttributes.Price));
                            totalDiscountAmount += ticketAttributes.discountedPrice;
                            transactionDetailList.Add(transactionDetail);
                        }
                        else
                        {
                            saveTransactionCommandResult.Id = -1;
                            saveTransactionCommandResult.Success = false;
                            saveTransactionCommandResult.ErrorMessage = "Ticket quantity not available";
                            return Task.FromResult<ICommandResult>(saveTransactionCommandResult);
                        }
                    }
                }

                IPDetail ipDetail = new IPDetail();
                if (!string.IsNullOrWhiteSpace(command.Ip))
                {
                    ipDetail = _iPDetailRepository.GetByIpAddress(command.Ip);
                    if (ipDetail == null)
                    {
                        IResponse<Contracts.Models.Integrations.IpApiResponse> ipApiResponse = _ipApi.GetUserLocationByIp(command.Ip).Result;
                        if (ipApiResponse.Result != null)
                        {
                            ipDetail = _iPDetailRepository.Save(new IPDetail
                            {
                                IPAddress = command.Ip,
                                CountryCode = ipApiResponse.Result.CountryCode,
                                CountryName = ipApiResponse.Result.Country,
                                RegionCode = ipApiResponse.Result.Region,
                                RegionName = ipApiResponse.Result.RegionName,
                                City = ipApiResponse.Result.City,
                                Zipcode = ipApiResponse.Result.ZipCode,
                                TimeZone = ipApiResponse.Result.Timezone,
                                Latitude = Convert.ToDecimal(ipApiResponse.Result.Latitude),
                                Longitude = Convert.ToDecimal(ipApiResponse.Result.Longitude)
                            });
                        }
                    }
                }
                transaction.AltId = Guid.NewGuid();
                transaction.ChannelId = command.ChannelId;
                transaction.CurrencyId = command.EventTicketAttributeList[0].CurrencyId;
                transaction.TotalTickets = Convert.ToInt16(command.EventTicketAttributeList.Sum(s => s.TotalTickets));
                transaction.GrossTicketAmount = grossTicketAmount;
                transaction.NetTicketAmount = netTicketAmount + command.boCustomerDetail.TransactionCharge;
                transaction.DiscountAmount = totalDiscountAmount;
                transaction.TransactionStatusId = TransactionStatus.UnderPayment;
                transaction.FirstName = command.boCustomerDetail.FirstName;
                transaction.LastName = command.boCustomerDetail.LastName;
                transaction.PhoneCode = command.boCustomerDetail.PhoneCode;
                transaction.PhoneNumber = command.boCustomerDetail.PhoneNumber;
                transaction.EmailId = command.boCustomerDetail.Email;
                transaction.CountryName = !string.IsNullOrWhiteSpace(command.boCustomerDetail.PhoneCode) ? _countryRepository.GetByPhoneCode(command.boCustomerDetail.PhoneCode).Name : "India";
                transaction.CreatedBy = command.UserAltId;
                transaction.CreatedUtc = DateTime.UtcNow;
                transaction.ModifiedBy = command.UserAltId;
                transaction.IPDetailId = ipDetail.Id;
                transaction.DiscountCode = command.EventTicketAttributeList[0].DiscountCode;
                transaction.ConvenienceCharges = command.boCustomerDetail.TransactionCharge;
                FIL.Contracts.DataModels.Transaction transactionResult = _transactionRepository.Save(transaction);

                foreach (var transactionDetail in transactionDetailList)
                {
                    transactionDetail.TransactionId = transactionResult.Id;
                    transactionDetail.ConvenienceCharges = command.boCustomerDetail.TransactionCharge / transactionDetailList.Count;
                    TransactionDetail transactionDetailResult = _transactionDetailRepository.Save(transactionDetail);
                    FIL.Contracts.DataModels.EventTicketAttribute eventTicketAttribute = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.EventTicketAttribute>(_eventTicketAttributeRepository.Get(transactionDetail.EventTicketAttributeId));
                    FIL.Contracts.DataModels.EventTicketDetail eventTicketDetail = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.EventTicketDetail>(_eventTicketDetailRepository.Get(eventTicketAttribute.EventTicketDetailId));
                    if (Convert.ToInt16(transactionDetail.TotalTickets) <= eventTicketAttribute.RemainingTicketForSale)
                    {
                        if (transactionDetail.TicketTypeId == (short)TicketType.Child || transactionDetail.TicketTypeId == (short)TicketType.SeasonChild)
                        {
                            eventTicketAttribute.RemainingTicketForSale -= transactionDetail.TotalTickets;
                            eventTicketAttribute.ChildQTY -= transactionDetail.TotalTickets;
                        }
                        else if (transactionDetail.TicketTypeId == (short)TicketType.SeniorCitizen)
                        {
                            eventTicketAttribute.RemainingTicketForSale -= transactionDetail.TotalTickets;
                            eventTicketAttribute.SRCitizenQTY -= transactionDetail.TotalTickets;
                        }
                        else
                        {
                            eventTicketAttribute.RemainingTicketForSale -= transactionDetail.TotalTickets;
                        }
                        _eventTicketAttributeRepository.Save(eventTicketAttribute);

                        if (eventTicketDetail.InventoryTypeId == InventoryType.Seated || eventTicketDetail.InventoryTypeId == InventoryType.SeatedWithSeatSelection || eventTicketDetail.InventoryTypeId == InventoryType.NoneSeated)
                        {
                            List<Contracts.Models.TMS.SeatDetail> seatDetail = new List<Contracts.Models.TMS.SeatDetail>();
                            if (command.SeatDetails != null)
                            {
                                seatDetail = AutoMapper.Mapper.Map<List<Contracts.Models.TMS.SeatDetail>>(command.SeatDetails.Where(w => w.EventTicketDetailId == eventTicketDetail.Id));
                            }
                            else
                            {
                                seatDetail = null;
                            }
                            var seatBlock = _seatBlockingProvider.BlockSeat(seatDetail, transactionDetail, eventTicketAttribute, eventTicketDetail, command.ModifiedBy, Channels.Boxoffice);
                            if (!seatBlock.Success && seatBlock.IsSeatSoldOut)
                            {
                                saveTransactionCommandResult.Id = -1;
                                saveTransactionCommandResult.Success = false;
                                saveTransactionCommandResult.ErrorMessage = "Seats are no longer available";
                                return Task.FromResult<ICommandResult>(saveTransactionCommandResult);
                            }
                        }
                    }
                    else
                    {
                        saveTransactionCommandResult.Id = -1;
                        saveTransactionCommandResult.Success = false;
                        saveTransactionCommandResult.ErrorMessage = "Ticket quantity not available";
                        return Task.FromResult<ICommandResult>(saveTransactionCommandResult);
                    }

                    _transactionDeliveryDetailRepository.Save(new TransactionDeliveryDetail
                    {
                        TransactionDetailId = transactionDetail.Id,
                        DeliveryTypeId = DeliveryTypes.VenuePickup,
                        PickupBy = 1,
                        SecondaryName = $"{command.boCustomerDetail.FirstName} {command.boCustomerDetail.LastName}",
                        SecondaryContact = $"{command.boCustomerDetail.PhoneCode}-{command.boCustomerDetail.PhoneNumber}",
                        SecondaryEmail = $"{command.boCustomerDetail.Email}"
                    });
                }

                _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                {
                    TransactionId = transactionResult.Id,
                    PaymentOptionId = PaymentOptions.Wallet,
                    PayConfNumber = "Wallet",
                    Amount = grossTicketAmount.ToString(),
                    PaymentDetail = "Boxoffice",
                    PaymentGatewayId = PaymentGateway.None,
                    ModifiedBy = command.UserAltId,
                });
                foreach (var customerDetail in command.PartPaymentDataList)
                {
                    _boCustomerDetailRepository.Save(new Contracts.DataModels.BoCustomerDetail
                    {
                        AltId = System.Guid.NewGuid(),
                        BankName = command.boCustomerDetail.BankName,
                        PaymentMode = customerDetail.paymentOptionId.ToString(),
                        TransactionId = transactionResult.Id,
                        ZipCode = command.boCustomerDetail.ZipCode,
                        ChequeNumber = command.boCustomerDetail.ChequeNumber,
                        ChequeDate = command.boCustomerDetail.PaymentOption == "Cheque" ? command.boCustomerDetail.ChequeDate : (DateTime?)null,
                        IsEnabled = true,
                        ModifiedBy = command.UserAltId,
                        CurrencyId = customerDetail.CurrencyId,
                        Value = customerDetail.Value,
                        PaymentOptionId = customerDetail.paymentOptionId
                    });
                };

                if (transactionResult.Id != -1)
                {
                    _transactionStatusUpdater.UpdateTranscationStatus(Convert.ToInt64(transactionResult.Id));
                }

                saveTransactionCommandResult.Id = transactionResult.Id;
                saveTransactionCommandResult.Success = true;
                return Task.FromResult<ICommandResult>(saveTransactionCommandResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                saveTransactionCommandResult.Id = -1;
                saveTransactionCommandResult.Success = false;
                saveTransactionCommandResult.ErrorMessage = ex.ToString();
                return Task.FromResult<ICommandResult>(saveTransactionCommandResult);
            }
        }
    }
}