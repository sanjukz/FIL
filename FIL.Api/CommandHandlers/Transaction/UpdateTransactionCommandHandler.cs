using FIL.Api.CommandHandlers;
using FIL.Api.Modules.SiteExtensions;
using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.Transaction
{
    public class UpdateTransactionCommandHandler : BaseCommandHandlerWithResult<UpdateTransactionCommand, UpdateTransactionCommandResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGeoCurrency _geoCurrency;
        private readonly ISaveGuestUserProvider _saveGuestUserProvider;
        private readonly IHttpContextAccessor _context;
        private readonly FIL.Logging.ILogger _logger;

        public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITicketFeeDetailRepository ticketFeeDetailRepository,
            ICountryRepository countryRepository,
            IUserRepository userRepository,
            ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository,
            ISaveGuestUserProvider saveGuestUserProvider,
            IMediator mediator, IGeoCurrency geoCurrency, ILogger logger,
            IHttpContextAccessor context) : base(mediator)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _userRepository = userRepository;
            _saveGuestUserProvider = saveGuestUserProvider;
            _geoCurrency = geoCurrency;
            _context = context;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(UpdateTransactionCommand command)
        {
            List<TransactionDetail> transactionDetailList = new List<TransactionDetail>();
            var transaction = _transactionRepository.Get(command.TransactionId);
            if (transaction != null && transaction.CreatedBy != null && transaction.ChannelId != Channels.Feel)
            {
                var user = _userRepository.GetByAltId(transaction.CreatedBy);
                user.PhoneCode = command.DeliveryDetail[0].PhoneCode;
                user.PhoneNumber = command.DeliveryDetail[0].PhoneNumber;
                User upadateUserResult = _userRepository.Save(user);
            }
            var transactionDetails = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.DataModels.TransactionDetail>>(_transactionDetailRepository.GetByTransactionId(command.TransactionId));
            var eventTicketAttributeIds = transactionDetails.Select(s => s.EventTicketAttributeId).Distinct();
            var eventTicketAttributes = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(_eventTicketAttributeRepository.GetByIds(eventTicketAttributeIds));
            var ticketFeeDetails = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketFeeDetail>>(_ticketFeeDetailRepository.GetByEventTicketAttributeIds(eventTicketAttributeIds));
            var isItinerary = TransactionType.Regular;
            decimal totalConvenienceCharge = 0, smsCharges = 0, totalServiceCharge = 0, totalDeliveryCharge = 0, grossTicketAmount = 0, transactionFee = 0, transactionFeeValue = 0, transactionFeeValueTypeId = 0, creditCardSurcharge = 0, creditCardSurchargeValue = 0, creditCardSurchargeValueTypeId = 0;

            Channels ChannelID = transaction.ChannelId;
            foreach (var transactionDetail in transactionDetails)
            {
                TransactionDetail transactionDetailMapper = transactionDetail;
                var eventTicketAttribute = eventTicketAttributes.Where(w => w.Id == transactionDetail.EventTicketAttributeId).FirstOrDefault();
                var ticketFeeDetailList = ticketFeeDetails.Where(w => w.EventTicketAttributeId == eventTicketAttribute.Id);
                var commandDeliveryDetail = command.DeliveryDetail.Where(w => w.EventTicketAttributeId == eventTicketAttribute.Id).FirstOrDefault();
                grossTicketAmount += transactionDetail.PricePerTicket * transactionDetail.TotalTickets;
            }

            foreach (var transactionDetail in transactionDetails)
            {
                TransactionDetail transactionDetailMapper = transactionDetail;
                if (transactionDetail.TransactionType == TransactionType.Itinerary)
                {
                    isItinerary = TransactionType.Itinerary;
                }
                var eventTicketAttribute = eventTicketAttributes.Where(w => w.Id == transactionDetail.EventTicketAttributeId).FirstOrDefault();
                var ticketFeeDetailList = ticketFeeDetails.Where(w => w.EventTicketAttributeId == eventTicketAttribute.Id).OrderBy(o => o.FeeId);

                // Convert TicketFeeDetails into geo currency...
                if (ChannelID == Channels.Feel)
                {
                    _geoCurrency.UpdateTicketFeeDetails(ticketFeeDetailList.ToList(), command.TargetCurrencyCode, eventTicketAttribute.CurrencyId);
                }
                var commandDeliveryDetail = command.DeliveryDetail.Where(w => w.EventTicketAttributeId == eventTicketAttribute.Id).FirstOrDefault();
                decimal prevConvenienceCharges = 0;
                decimal DeliveryCharge = 0, ServiceCharge = 0, ConvenienceCharge = 0;
                decimal convenienceCharges = 0, serviceCharge = 0, pahCharge = 0, unitLevelSMSCharge = 0, unitLevelTransactionFee = 0;
                foreach (var ticketFeeDetail in ticketFeeDetailList)
                {
                    if (ticketFeeDetail.FeeId == (int)FeeType.ConvenienceCharge)
                    {
                        if (ticketFeeDetail.ValueTypeId == (int)ValueTypes.Percentage)
                        {
                            convenienceCharges = ((ticketFeeDetail.Value * (transactionDetail.PricePerTicket * transactionDetail.TotalTickets)) / 100);
                            prevConvenienceCharges = convenienceCharges;
                        }
                        else if (ticketFeeDetail.ValueTypeId == (int)ValueTypes.Flat)
                        {
                            convenienceCharges = ticketFeeDetail.Value * transactionDetail.TotalTickets;
                            prevConvenienceCharges = convenienceCharges;
                        }
                    }

                    if (ticketFeeDetail.FeeId == (int)FeeType.SMSCharge)
                    {
                        smsCharges = ticketFeeDetail.Value;
                    }

                    if (ticketFeeDetail.FeeId == (int)FeeType.TransactionFee)
                    {
                        transactionFeeValue = ticketFeeDetail.Value;
                        transactionFeeValueTypeId = ticketFeeDetail.ValueTypeId;
                    }

                    if (ticketFeeDetail.FeeId == (int)FeeType.CreditCardSurcharge)
                    {
                        creditCardSurchargeValue = ticketFeeDetail.Value;
                        creditCardSurchargeValueTypeId = ticketFeeDetail.ValueTypeId;
                    }

                    if (transactionFeeValueTypeId == (int)ValueTypes.Percentage)
                    {
                        transactionFee = ((transactionFeeValue * (totalConvenienceCharge + smsCharges)) / 100);
                    }
                    else if (transactionFeeValueTypeId == (int)ValueTypes.Flat)
                    {
                        transactionFee = transactionFeeValue;
                    }

                    if (creditCardSurchargeValueTypeId == (int)ValueTypes.Percentage)
                    {
                        creditCardSurcharge = ((creditCardSurchargeValue * (grossTicketAmount + transactionFee)) / 100);
                    }
                    else if (creditCardSurchargeValue == (int)ValueTypes.Flat)
                    {
                        creditCardSurcharge = creditCardSurchargeValue;
                    }
                    unitLevelSMSCharge = (smsCharges / transactionDetails.Count());
                    unitLevelTransactionFee = ((transactionFee + creditCardSurcharge) / transactionDetails.Count());
                    ConvenienceCharge = convenienceCharges + unitLevelSMSCharge + unitLevelTransactionFee;

                    if (ticketFeeDetail.FeeId == (int)FeeType.ServiceCharge)
                    {
                        if (ticketFeeDetail.ValueTypeId == (int)ValueTypes.Percentage)
                        {
                            serviceCharge = ((ticketFeeDetail.Value * prevConvenienceCharges) / 100);
                            prevConvenienceCharges = 0;
                        }
                        else if (ticketFeeDetail.ValueTypeId == (int)ValueTypes.Flat)
                        {
                            serviceCharge = ticketFeeDetail.Value * transactionDetail.TotalTickets;
                        }
                    }
                    ServiceCharge = serviceCharge;

                    if (commandDeliveryDetail.DeliveryTypeId == DeliveryTypes.PrintAtHome && ticketFeeDetail.FeeId == (int)FeeType.PrintAtHomeCharge)
                    {
                        pahCharge = ticketFeeDetail.Value;
                    }
                    DeliveryCharge = pahCharge;
                }
                transactionDetailMapper.ConvenienceCharges = ConvenienceCharge;
                transactionDetailMapper.ServiceCharge = ServiceCharge;
                transactionDetailMapper.DeliveryCharges = DeliveryCharge;
                totalDeliveryCharge += DeliveryCharge;
                totalServiceCharge += ServiceCharge;
                totalConvenienceCharge += ConvenienceCharge;
                transactionDetailList.Add(transactionDetailMapper);
            }

            if (transactionFeeValueTypeId == (int)ValueTypes.Percentage)
            {
                transactionFee = ((transactionFeeValue * (totalConvenienceCharge + smsCharges)) / 100);
            }
            else if (transactionFeeValueTypeId == (int)ValueTypes.Flat)
            {
                transactionFee = transactionFeeValue;
            }

            if (creditCardSurchargeValueTypeId == (int)ValueTypes.Percentage)
            {
                creditCardSurcharge = ((creditCardSurchargeValue * (grossTicketAmount + transactionFee)) / 100);
            }
            else if (creditCardSurchargeValue == (int)ValueTypes.Flat)
            {
                creditCardSurcharge = creditCardSurchargeValue;
            }

            if (transactionFee > 0)
            {
                transaction.ConvenienceCharges = transactionFee + creditCardSurcharge;
                transaction.ServiceCharge = totalServiceCharge;
                transaction.DeliveryCharges = totalDeliveryCharge;
                transaction.GrossTicketAmount = grossTicketAmount;
                transaction.NetTicketAmount = ((grossTicketAmount + transactionFee + creditCardSurcharge) - transaction.DiscountAmount);
            }
            else
            {
                transaction.ConvenienceCharges = totalConvenienceCharge + transactionFee + creditCardSurcharge;
                transaction.ServiceCharge = totalServiceCharge;
                transaction.DeliveryCharges = totalDeliveryCharge;
                transaction.GrossTicketAmount = grossTicketAmount;
                transaction.NetTicketAmount = ((grossTicketAmount + totalConvenienceCharge + totalServiceCharge + totalDeliveryCharge + transactionFee + creditCardSurcharge) - transaction.DiscountAmount);
            }
            if (ChannelID == Channels.Feel)
            {
                _geoCurrency.UpdateTransactionUpdates(transaction, command.TargetCurrencyCode);
            }
            FIL.Contracts.DataModels.Transaction transactionResult = _transactionRepository.Save(transaction);
            foreach (var transactionDetail in transactionDetailList)
            {
                TransactionDetail transactionDetailResult = _transactionDetailRepository.Save(transactionDetail);
                try
                {
                    if (ChannelID == Channels.Feel)
                    {
                        var currentETA = command.EventTicketAttributeList.Where(s => s.Id == transactionDetail.EventTicketAttributeId && (transactionDetail.TicketTypeId != null ? (TicketType)transactionDetail.TicketTypeId : TicketType.Regular) == s.TicketType).FirstOrDefault();
                        if (currentETA != null && currentETA.GuestDetails != null)
                        {
                            foreach (FIL.Contracts.Commands.Transaction.GuestUserDetail currentGuestDetail in currentETA.GuestDetails)
                            {
                                _saveGuestUserProvider.SaveGuestUsers(currentGuestDetail, transactionDetail);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, e);
                }
            }

            var transactionDeliveryDetails = _transactionDeliveryDetailRepository.GetByTransactionDetailIds(transactionDetails.Select(s => s.Id).Distinct());
            foreach (var deliveryDetail in command.DeliveryDetail)
            {
                var transactionDeliveryDetail = transactionDeliveryDetails.Where(w => w.TransactionDetailId == transactionDetails.Where(c => c.EventTicketAttributeId == deliveryDetail.EventTicketAttributeId).Select(s => s.Id).FirstOrDefault()).FirstOrDefault();
                if (transactionDeliveryDetail == null)
                {
                    _transactionDeliveryDetailRepository.Save(new TransactionDeliveryDetail
                    {
                        TransactionDetailId = transactionDetails.Where(w => w.EventTicketAttributeId == deliveryDetail.EventTicketAttributeId).Select(s => s.Id).FirstOrDefault(),
                        DeliveryTypeId = deliveryDetail.DeliveryTypeId,
                        PickupBy = Convert.ToInt16(string.IsNullOrWhiteSpace(deliveryDetail.RepresentativeFirstName) ? 0 : 1),
                        SecondaryName = $"{deliveryDetail.FirstName} {deliveryDetail.LastName}",
                        SecondaryContact = $"{deliveryDetail.PhoneCode}-{deliveryDetail.PhoneNumber}",
                        SecondaryEmail = deliveryDetail.DeliveryTypeId == DeliveryTypes.Courier ? deliveryDetail.Email : deliveryDetail.Email,
                        PickUpAddress = command.PickUpAddress
                    });
                }
                else
                {
                    transactionDeliveryDetail.SecondaryName = $"{deliveryDetail.FirstName} {deliveryDetail.LastName}";
                    transactionDeliveryDetail.SecondaryContact = $"{deliveryDetail.PhoneCode}-{deliveryDetail.PhoneNumber}";
                    transactionDeliveryDetail.SecondaryEmail = deliveryDetail.DeliveryTypeId == DeliveryTypes.Courier ? deliveryDetail.Email : deliveryDetail.Email;
                    transactionDeliveryDetail.PickUpAddress = command.PickUpAddress;
                    _transactionDeliveryDetailRepository.Save(transactionDeliveryDetail);
                }
            }
            var transactionresult = _transactionRepository.Get(command.TransactionId);

            UpdateTransactionCommandResult updateTransactionCommandResult = new UpdateTransactionCommandResult();
            updateTransactionCommandResult.Id = transactionresult.Id;
            updateTransactionCommandResult.CurrencyId = transactionresult.CurrencyId;
            updateTransactionCommandResult.GrossTicketAmount = transactionresult.GrossTicketAmount;
            updateTransactionCommandResult.DeliveryCharges = transactionresult.DeliveryCharges;
            updateTransactionCommandResult.ConvenienceCharges = transactionresult.ConvenienceCharges;
            updateTransactionCommandResult.ServiceCharge = transactionresult.ServiceCharge;
            updateTransactionCommandResult.DiscountAmount = transactionresult.DiscountAmount;
            updateTransactionCommandResult.NetTicketAmount = transactionresult.NetTicketAmount;

            return Task.FromResult<ICommandResult>(updateTransactionCommandResult);
        }
    }
}