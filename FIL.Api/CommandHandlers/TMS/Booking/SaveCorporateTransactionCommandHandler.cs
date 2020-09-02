using FIL.Api.Integrations;
using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS.Booking;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS.Booking
{
    public class SaveCorporateTransactionCommandHandler : BaseCommandHandlerWithResult<SaveCorporateTransactionCommand, SaveCorporateTransactionCommandResult>
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ICorporateTicketAllocationDetailRepository _corporateTicketAllocationDetailRepository;
        private readonly ICorporateTransactionDetailRepository _corporateTransactionDetailRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITransactionSeatDetailRepository _transactionSeatDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IIPDetailRepository _iPDetailRepository;
        private readonly IIpApi _ipApi;
        private readonly Logging.ILogger _logger;
        private readonly IMatchLayoutCompanionSeatMappingRepository _matchLayoutCompanionSeatMappingRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ISeatBlockingProvider _seatBlockingProvider;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;

        public SaveCorporateTransactionCommandHandler(
            ISponsorRepository sponsorRepository,
            ICorporateTicketAllocationDetailRepository corporateTicketAllocationDetailRepository,
            ICorporateTransactionDetailRepository corporateTransactionDetailRepository,
            ICountryRepository countryRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            ITransactionSeatDetailRepository transactionSeatDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventDetailRepository eventDetailRepository,
            IIPDetailRepository iPDetailRepository,
            IIpApi ipApi,
            Logging.ILogger logger,
            IMatchLayoutCompanionSeatMappingRepository matchLayoutCompanionSeatMappingRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ISeatBlockingProvider seatBlockingProvider,
            ITicketFeeDetailRepository ticketFeeDetailRepository,

        IMediator mediator) : base(mediator)
        {
            _sponsorRepository = sponsorRepository;
            _countryRepository = countryRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _corporateTicketAllocationDetailRepository = corporateTicketAllocationDetailRepository;
            _corporateTransactionDetailRepository = corporateTransactionDetailRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _transactionSeatDetailRepository = transactionSeatDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventDetailRepository = eventDetailRepository;
            _iPDetailRepository = iPDetailRepository;
            _ipApi = ipApi;
            _logger = logger;
            _matchLayoutCompanionSeatMappingRepository = matchLayoutCompanionSeatMappingRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _seatBlockingProvider = seatBlockingProvider;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
        }

        protected override Task<ICommandResult> Handle(SaveCorporateTransactionCommand command)
        {
            SaveCorporateTransactionCommandResult saveCorporateTransactionCommandResult = new SaveCorporateTransactionCommandResult();
            try
            {
                List<TransactionDetail> transactionDetailList = new List<TransactionDetail>();
                List<CorporateTransactionDetail> corporateTransactionDetailList = new List<CorporateTransactionDetail>();
                FIL.Contracts.DataModels.Transaction transaction = new FIL.Contracts.DataModels.Transaction();
                decimal grossTicketAmount = 0, netTicketAmount = 0, totalDiscountAmount = 0, totalConvenienceCharge = 0, totalServiceCharge = 0, totalDeliveryCharge = 0, deliveryCharge = 0;
                foreach (var item in command.CorporateTicketDetails)
                {
                    var corporateTicketAllocationDetail = _corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsor(item.EventTicketAttributeId, item.SponsorId);
                    if (Convert.ToInt16(item.TotalTickets) <= corporateTicketAllocationDetail.RemainingTickets)
                    {
                        TransactionDetail transactionDetail = new TransactionDetail();
                        transactionDetail.EventTicketAttributeId = corporateTicketAllocationDetail.EventTicketAttributeId;
                        transactionDetail.TotalTickets = Convert.ToInt16(item.TotalTickets);
                        transactionDetail.PricePerTicket = item.PricePerTicket;
                        transactionDetail.DiscountAmount = item.DiscountAmount;
                        transactionDetail.ConvenienceCharges = item.ConvenceCharge;
                        transactionDetail.ServiceCharge = item.ServiceTax;
                        transactionDetail.DeliveryCharges = deliveryCharge;
                        transactionDetail.TicketTypeId = (short)TicketType.Regular;
                        totalConvenienceCharge += command.TransactingOptionId == TransactingOption.Paid ? item.ConvenceCharge : 0;
                        totalServiceCharge += command.TransactingOptionId == TransactingOption.Paid ? item.ServiceTax : 0;
                        totalDeliveryCharge += command.TransactingOptionId == TransactingOption.Paid ? deliveryCharge : 0;
                        grossTicketAmount += command.TransactingOptionId == TransactingOption.Paid ? ((item.TotalTickets * item.PricePerTicket)) : 0;
                        totalDiscountAmount += command.TransactingOptionId == TransactingOption.Paid ? item.DiscountAmount : 0;
                        netTicketAmount += command.TransactingOptionId == TransactingOption.Paid ? (((item.TotalTickets * item.PricePerTicket) + item.ConvenceCharge + item.ServiceTax) - (item.DiscountAmount)) : 0;
                        transactionDetailList.Add(transactionDetail);
                    }
                    else
                    {
                        saveCorporateTransactionCommandResult.Id = -1;
                        saveCorporateTransactionCommandResult.Success = false;
                        saveCorporateTransactionCommandResult.ErrorMessage = "Ticket quantity not available";
                        return Task.FromResult<ICommandResult>(saveCorporateTransactionCommandResult);
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
                                Longitude = Convert.ToDecimal(ipApiResponse.Result.Longitude),
                                ModifiedBy = command.ModifiedBy
                            });
                        }
                    }
                }
                transaction.AltId = Guid.NewGuid();
                transaction.ChannelId = command.ChannelId;
                transaction.CurrencyId = command.CorporateTicketDetails[0].CurrencyId;
                transaction.TotalTickets = Convert.ToInt16(command.CorporateTicketDetails.Sum(s => s.TotalTickets));
                transaction.GrossTicketAmount = grossTicketAmount;
                transaction.NetTicketAmount = netTicketAmount;
                transaction.DiscountAmount = totalDiscountAmount;
                transaction.ConvenienceCharges = totalConvenienceCharge;
                transaction.ServiceCharge = totalServiceCharge;
                transaction.TransactionStatusId = TransactionStatus.UnderPayment;
                transaction.FirstName = !string.IsNullOrWhiteSpace(command.CorporateTicketDetails[0].FirstName) ? command.CorporateTicketDetails[0].FirstName : ""; ;
                transaction.LastName = !string.IsNullOrWhiteSpace(command.CorporateTicketDetails[0].LastName) ? command.CorporateTicketDetails[0].LastName : ""; ;
                transaction.PhoneCode = !string.IsNullOrWhiteSpace(command.CorporateTicketDetails[0].PhoneCode) ? command.CorporateTicketDetails[0].PhoneCode : ""; ;
                transaction.PhoneNumber = !string.IsNullOrWhiteSpace(command.CorporateTicketDetails[0].PhoneNumber) ? command.CorporateTicketDetails[0].PhoneNumber : ""; ;
                transaction.EmailId = !string.IsNullOrWhiteSpace(command.CorporateTicketDetails[0].Email) ? command.CorporateTicketDetails[0].Email : "";
                transaction.CountryName = !string.IsNullOrWhiteSpace(command.CorporateTicketDetails[0].PhoneCode) ? !string.IsNullOrWhiteSpace(_countryRepository.GetByPhoneCode(command.CorporateTicketDetails[0].PhoneCode).Name) ? _countryRepository.GetByPhoneCode(command.CorporateTicketDetails[0].PhoneCode).Name : "India" : "India";
                transaction.ModifiedBy = command.ModifiedBy;
                transaction.IPDetailId = ipDetail.Id;
                FIL.Contracts.DataModels.Transaction transactionResult = _transactionRepository.Save(transaction);
                FIL.Contracts.DataModels.CorporateTransactionDetail corporateTransactionDetail = new FIL.Contracts.DataModels.CorporateTransactionDetail();
                foreach (var transactionDetail in transactionDetailList)
                {
                    transactionDetail.TransactionId = transactionResult.Id;
                    transactionDetail.ModifiedBy = command.ModifiedBy;
                    TransactionDetail transactionDetailResult = _transactionDetailRepository.Save(transactionDetail);

                    corporateTransactionDetail.AltId = new Guid();
                    corporateTransactionDetail.TransactionId = transactionResult.Id;
                    corporateTransactionDetail.EventTicketAttributeId = transactionDetail.EventTicketAttributeId;
                    corporateTransactionDetail.SponsorId = command.CorporateTicketDetails[0].SponsorId;
                    corporateTransactionDetail.TotalTickets = transactionDetail.TotalTickets;
                    corporateTransactionDetail.Price = transactionDetail.PricePerTicket;
                    corporateTransactionDetail.TransactingOptionId = (short)(TransactingOption)command.TransactingOptionId;
                    corporateTransactionDetail.IsEnabled = true;
                    corporateTransactionDetail.ModifiedBy = command.ModifiedBy;
                    CorporateTransactionDetail corporateTransactionDetailResult = _corporateTransactionDetailRepository.Save(corporateTransactionDetail);

                    FIL.Contracts.DataModels.CorporateTicketAllocationDetail corporateTicketAllocationDetail = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.CorporateTicketAllocationDetail>(_corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsor(transactionDetail.EventTicketAttributeId, command.CorporateTicketDetails[0].SponsorId));
                    var eventTicketAttributes = _eventTicketAttributeRepository.Get(corporateTicketAllocationDetail.EventTicketAttributeId);
                    FIL.Contracts.DataModels.EventTicketDetail eventTicketDetail = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.EventTicketDetail>(_eventTicketDetailRepository.Get(eventTicketAttributes.EventTicketDetailId));

                    if (Convert.ToInt16(transactionDetail.TotalTickets) <= corporateTicketAllocationDetail.RemainingTickets)
                    {
                        corporateTicketAllocationDetail.RemainingTickets -= transactionDetail.TotalTickets;
                        _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

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
                            var seatBlock = _seatBlockingProvider.BlockSeat(seatDetail, transactionDetail, eventTicketAttributes, eventTicketDetail, command.ModifiedBy, Channels.Corporate);
                            if (!seatBlock.Success && seatBlock.IsSeatSoldOut)
                            {
                                saveCorporateTransactionCommandResult.Id = -1;
                                saveCorporateTransactionCommandResult.Success = false;
                                saveCorporateTransactionCommandResult.ErrorMessage = "Seats are no longer available";
                                return Task.FromResult<ICommandResult>(saveCorporateTransactionCommandResult);
                            }
                        }
                    }
                    else
                    {
                        saveCorporateTransactionCommandResult.Id = -1;
                        saveCorporateTransactionCommandResult.Success = false;
                        saveCorporateTransactionCommandResult.ErrorMessage = "Ticket quantity not available";
                        return Task.FromResult<ICommandResult>(saveCorporateTransactionCommandResult);
                    }

                    _transactionDeliveryDetailRepository.Save(new TransactionDeliveryDetail
                    {
                        TransactionDetailId = transactionDetail.Id,
                        DeliveryTypeId = DeliveryTypes.VenuePickup,
                        PickupBy = 1,
                        SecondaryName = command.CorporateTicketDetails[0].SponsorName,
                        SecondaryContact = (!string.IsNullOrWhiteSpace(command.CorporateTicketDetails[0].PhoneCode) ? command.CorporateTicketDetails[0].PhoneCode : "") + "-" + (!string.IsNullOrWhiteSpace(command.CorporateTicketDetails[0].PhoneNumber) ? command.CorporateTicketDetails[0].PhoneNumber : ""),
                        SecondaryEmail = !string.IsNullOrWhiteSpace(command.CorporateTicketDetails[0].Email) ? command.CorporateTicketDetails[0].Email : "",
                        ModifiedBy = command.ModifiedBy
                    });
                }

                _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                {
                    TransactionId = transactionResult.Id,
                    PayConfNumber = command.PaymentType,
                    Amount = grossTicketAmount.ToString(),
                    PaymentDetail = command.TransactingOptionId == TransactingOption.Paid ? "Paid" : "Complimentary",
                    ModifiedBy = command.ModifiedBy,
                });
                saveCorporateTransactionCommandResult.Id = transactionResult.Id;
                saveCorporateTransactionCommandResult.Success = true;
                return Task.FromResult<ICommandResult>(saveCorporateTransactionCommandResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                saveCorporateTransactionCommandResult.Id = -1;
                saveCorporateTransactionCommandResult.Success = false;
                saveCorporateTransactionCommandResult.ErrorMessage = ex.ToString();
                return Task.FromResult<ICommandResult>(saveCorporateTransactionCommandResult);
            }
        }
    }
}