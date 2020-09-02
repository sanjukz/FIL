using FIL.Api.Integrations;
using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.ASI;
using FIL.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Api.Providers.ASI
{
    public interface IASIBookingProvider
    {
        Task<ASIBookingResponse> saveASIData(ASIBooking aSIBooking);
    }

    public class ASIBookingProvider : Service<string>, IASIBookingProvider
    {
        private readonly IGuestDetailRepository _guestDetailRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IASIMonumentDetailRepository _aSIMonumentDetailRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IASITransactionDetailTimeSlotIdMappingRepository _aSITransactionDetailTimeSlotIdMappingRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ICustomerDocumentTypeRepository _customerDocumentTypeRepository;
        private readonly IASIMonumentEventTableMappingRepository _aSIMonumentEventTableMappingRepository;
        private readonly IASIMonumentRepository _aSIMonumentRepository;
        private readonly IEventTimeSlotMappingRepository _eventTimeSlotMappingRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ISaveGuestUserProvider _saveGuestUserProvider;

        public ASIBookingProvider(ILogger logger, ISettings settings,
            IGuestDetailRepository guestDetailRepository,
            ICountryRepository countryRepository,
            IASIMonumentDetailRepository aSIMonumentDetailRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
             IEventTimeSlotMappingRepository eventTimeSlotMappingRepository,
            ITransactionRepository transactionRepository,
            IASIMonumentRepository aSIMonumentRepository,
            IUserCardDetailRepository userCardDetailRepository,
            IASIMonumentEventTableMappingRepository aSIMonumentEventTableMappingRepository,
            IEventRepository eventRepository,
            ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            ICustomerDocumentTypeRepository customerDocumentTypeRepository,
            ITransactionDetailRepository transactionDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ISaveGuestUserProvider saveGuestUserProvider,
            IASITransactionDetailTimeSlotIdMappingRepository aSITransactionDetailTimeSlotIdMappingRepository
            ) : base(logger, settings)
        {
            _guestDetailRepository = guestDetailRepository;
            _countryRepository = countryRepository;
            _eventDetailRepository = eventDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _aSIMonumentEventTableMappingRepository = aSIMonumentEventTableMappingRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _customerDocumentTypeRepository = customerDocumentTypeRepository;
            _eventTimeSlotMappingRepository = eventTimeSlotMappingRepository;
            _aSITransactionDetailTimeSlotIdMappingRepository = aSITransactionDetailTimeSlotIdMappingRepository;
            _eventRepository = eventRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _aSIMonumentRepository = aSIMonumentRepository;
            _aSIMonumentDetailRepository = aSIMonumentDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _userCardDetailRepository = userCardDetailRepository;
            _saveGuestUserProvider = saveGuestUserProvider;
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        private static string GenerateSHA512Hash(string text)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public async Task<ASIBookingResponse> saveASIData(ASIBooking aSIBooking)
        {
            var transactionData = _transactionRepository.Get(aSIBooking.TransactionId);
            var transactionDetails = _transactionDetailRepository.GetByTransactionId(aSIBooking.TransactionId);
            List<Visitor> visitors = new List<Visitor>();
            List<ASIHash> aSIHashes = new List<ASIHash>();
            RootObject rootObject = new RootObject();
            foreach (FIL.Contracts.DataModels.TransactionDetail currentTransactionDetail in transactionDetails)
            {
                var clientETAData = aSIBooking.EventTicketAttributeList.Where(s => s.Id == currentTransactionDetail.EventTicketAttributeId && (TicketType)currentTransactionDetail.TicketTypeId == s.TicketType).FirstOrDefault();
                if (clientETAData != null)
                {
                    foreach (FIL.Contracts.Commands.Transaction.GuestUserDetail currentGuestDetail in clientETAData.GuestDetails)
                    {
                        _saveGuestUserProvider.SaveGuestUsers(currentGuestDetail, currentTransactionDetail);
                    }

                    _aSITransactionDetailTimeSlotIdMappingRepository.Save(new FIL.Contracts.DataModels.ASI.ASITransactionDetailTimeSlotIdMapping
                    {
                        EventTimeSlotMappingId = (long)clientETAData.VisitTimeId,
                        TransactionDetailId = currentTransactionDetail.Id,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow
                    });

                    _transactionDeliveryDetailRepository.Save(new TransactionDeliveryDetail
                    {
                        TransactionDetailId = currentTransactionDetail.Id,
                        DeliveryTypeId = DeliveryTypes.PrintAtHome,
                        PickupBy = 1,
                        SecondaryName = "Zoonga",
                        SecondaryContact = "Zoonga",
                        SecondaryEmail = transactionData.EmailId,
                        PickUpAddress = "Zoonga"
                    });
                }
            }
            // This is separate loop for API request payload..... I don't think we need to manage data from above loop... Better to keep it separate
            foreach (FIL.Contracts.DataModels.TransactionDetail currentTransactionDetail in transactionDetails)
            {
                var guestData = _guestDetailRepository.GetByTransactionDetailId(currentTransactionDetail.Id);
                var asiTicketAttribute = aSIBooking.EventTicketAttributeList.Where(s => s.Id == currentTransactionDetail.EventTicketAttributeId).FirstOrDefault();
                if (guestData.Any())
                {
                    var currentETA = _eventTicketAttributeRepository.Get(currentTransactionDetail.EventTicketAttributeId);
                    var currentETD = _eventTicketDetailRepository.Get(currentETA.EventTicketDetailId);
                    var currentTc = _ticketCategoryRepository.Get((int)currentETD.TicketCategoryId);
                    var currentED = _eventDetailRepository.Get(currentETD.EventDetailId);
                    var currentE = _eventRepository.Get(currentED.EventId);
                    var asiETableMapping = _aSIMonumentEventTableMappingRepository.GetByEventId(currentE.Id);
                    var asiMonument = _aSIMonumentRepository.Get(asiETableMapping.ASIMonumentId);
                    var asiMonumentDetail = _aSIMonumentDetailRepository.GetByNameAndMonumentId(currentED.Name, asiMonument.Id);
                    var asiTimeSlot = _aSITransactionDetailTimeSlotIdMappingRepository.GetByTransactionDetailId(currentTransactionDetail.Id);
                    var eventTimeSlot = _eventTimeSlotMappingRepository.Get(asiTimeSlot.EventTimeSlotMappingId);
                    //api.key |[monument.code | optional | date | age | country | amount] |[monument.code | optional | date | age | country | amount] | api.salt
                    foreach (FIL.Contracts.DataModels.GuestDetail currentGuestDetail in guestData)
                    {
                        Visitor visitor = new Visitor();
                        ASIHash aSIHash = new ASIHash();
                        Identity indentity = new Identity();
                        Monument monument = new Monument();
                        Nationality nationality = new Nationality();
                        Timeslot timeslot = new Timeslot();
                        visitor.Identity = indentity;
                        visitor.Monument = monument;
                        visitor.Monument.Timeslot = timeslot;
                        visitor.Nationality = nationality;
                        var custerDocType = _customerDocumentTypeRepository.Get(currentGuestDetail.CustomerDocumentTypeId);
                        visitor.Age = Convert.ToInt32(currentGuestDetail.Age);
                        visitor.VisitorId = currentGuestDetail.Id.ToString();
                        visitor.Amount = currentTransactionDetail.PricePerTicket;
                        visitor.Gender = currentGuestDetail.GenderId.ToString();
                        visitor.Name = currentGuestDetail.FirstName + " " + currentGuestDetail.LastName;
                        visitor.Date = ((System.DateTime)currentTransactionDetail.VisitDate).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                        visitor.Identity.Type = custerDocType.DocumentType;
                        visitor.Identity.No = currentGuestDetail.DocumentNumber;
                        visitor.Monument.Code = "TAJ";
                        visitor.Monument.Optional = asiMonumentDetail.IsOptional;
                        visitor.Monument.Main = true;
                        visitor.Monument.Timeslot.Id = eventTimeSlot.TimeSlotId;
                        visitor.Nationality.Group = currentTc.Name;
                        visitor.Nationality.Country = asiTicketAttribute == null ? "India" : asiTicketAttribute.ASIUserSelectedCountry;
                        visitors.Add(visitor);

                        aSIHash.Code = asiMonument.Code;
                        aSIHash.IsOptional = asiMonumentDetail.IsOptional;
                        aSIHash.Date = (System.DateTime)currentTransactionDetail.VisitDate;
                        aSIHash.Nationality = currentTc.Name;
                        aSIHash.Age = Convert.ToInt32(currentGuestDetail.Age);
                        aSIHash.Amount = currentTransactionDetail.PricePerTicket;
                        aSIHash.IdentityType = "Passport";
                        aSIHash.IdentityNumber = "SADFG";
                        aSIHash.VisitorId = currentGuestDetail.Id.ToString();
                        aSIHashes.Add(aSIHash);
                    }
                }
            }

            var ticketHash = "";
            foreach (FIL.Contracts.Models.ASI.ASIHash currentHash in aSIHashes)
            {
                TimeZoneInfo INDIAN_ZONE;
                try
                {
                    INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                }
                catch (Exception e)
                {
                    INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
                }
                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(currentHash.Date, INDIAN_ZONE);
                var time = indianTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                ticketHash = ticketHash + "[" + currentHash.Code + "|" + (!currentHash.IsOptional).ToString().ToLower() + "|" + currentHash.IsOptional.ToString().ToLower() + "|" + indianTime.ToString("yyyy-MM-dd") + "|" + currentHash.Age + "|" + currentHash.Nationality + "|" + currentHash.Amount.ToString("0.00") + "]" + "|";
                //ticketHash = ticketHash + "[" + "TAJ" + "|" + time + "|" + currentHash.IdentityType + "|" + currentHash.IdentityNumber + "|" + currentHash.Age + "|" + currentHash.Nationality + "|" + currentHash.Amount.ToString("0.00") + "|" + currentHash.VisitorId + "]" + "|";
            }
            var bookingHashRequestData = _settings.GetConfigSetting<string>(SettingKeys.Integration.ASI.APIKey) + "|" + ticketHash + _settings.GetConfigSetting<string>(SettingKeys.Integration.ASI.APISalt);
            var bookingHash = GenerateSHA512Hash(bookingHashRequestData);
            rootObject.TransactionId = aSIBooking.TransactionId.ToString();
            rootObject.Hash = bookingHash.ToLower();
            rootObject.Visitors = visitors;
            rootObject.Email = transactionData.EmailId == "asi@zoonga.com" ? transactionData.PhoneNumber : transactionData.EmailId;
            UserCardDetail userCardDetail = _userCardDetailRepository.GetByUserCardNumber(string.Empty, aSIBooking.UserId);
            if (userCardDetail == null)
            {
                UserCardDetail obj = new UserCardDetail
                {
                    UserId = aSIBooking.UserId,
                    AltId = new Guid(),
                    NameOnCard = string.Empty,
                    CardNumber = string.Empty,
                    ExpiryMonth = 12,
                    ExpiryYear = 2020,
                    CardTypeId = CardType.None
                };
                userCardDetail = _userCardDetailRepository.Save(obj);
            }

            _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail // Create transaction payment request before calling booking POST API request...
            {
                TransactionId = aSIBooking.TransactionId,
                PaymentOptionId = PaymentOptions.None,
                PaymentGatewayId = PaymentGateway.Payu,
                UserCardDetailId = userCardDetail.Id,
                RequestType = "Charge Posting",
                Amount = transactionData.NetTicketAmount.ToString(),
                PayConfNumber = "",
                PaymentDetail = "{\"Request\":" + Newtonsoft.Json.JsonConvert.SerializeObject(rootObject) + "{\"hashparameter\":" + bookingHashRequestData + "}}",
            });
            var baseAddress = _settings.GetConfigSetting<string>(SettingKeys.Integration.ASI.IntegrationRoot).ToString() + "" + _settings.GetConfigSetting<string>(SettingKeys.Integration.ASI.APIKey).ToString();

            return new ASIBookingResponse
            {
                RootObject = rootObject,
                ReturnUrl = baseAddress
            };
        }
    }
}