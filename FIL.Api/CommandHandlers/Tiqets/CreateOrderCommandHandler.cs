using FIL.Api.Repositories;
using FIL.Api.Repositories.Tiqets;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Tiqets;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Tiqets
{
    public class CreateOrderCommandHandler : BaseCommandHandlerWithResult<CreateOrderCommand, CreateOrderCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITiqetEventTicketDetailMappingRepository _tiqetEventTicketDetailMappingRepository;
        private readonly ITiqetVariantDetailRepository _tiqetVariantDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITiqetEventDetailMappingRepository _tiqetEventDetailMappingRepository;

        public CreateOrderCommandHandler(
       ILogger logger, ISettings settings, IEventTicketAttributeRepository eventTicketAttributeRepository, IEventTicketDetailRepository eventTicketDetailRepository, IUserRepository userRepository, ITiqetEventTicketDetailMappingRepository tiqetEventTicketDetailMappingRepository, ITiqetVariantDetailRepository tiqetVariantDetailRepository, ITiqetEventDetailMappingRepository tiqetEventDetailMappingRepository, IEventDetailRepository eventDetailRepository,
        IMediator mediator) : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _userRepository = userRepository;
            _tiqetVariantDetailRepository = tiqetVariantDetailRepository;
            _tiqetEventTicketDetailMappingRepository = tiqetEventTicketDetailMappingRepository;
            _tiqetEventDetailMappingRepository = tiqetEventDetailMappingRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        protected override async Task<ICommandResult> Handle(CreateOrderCommand command)
        {
            CreateOrderCommandResult commandResults = new CreateOrderCommandResult();
            var user = _userRepository.GetByAltId(command.UserAltId);
            var eventTicketAttributeIds = command.EventTicketAttributeList.Select(s => s.Id).Distinct();
            var eventTicketAttributes = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(_eventTicketAttributeRepository.GetByIds(eventTicketAttributeIds));
            var allEventTicketDetails = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(_eventTicketDetailRepository.GetByIds(eventTicketAttributes.Select(s => s.EventTicketDetailId))).ToList();
            var tiqetEventDetail = _tiqetEventDetailMappingRepository.GetByEventDetailId(allEventTicketDetails[0].EventDetailId);

            CreateOrderRequestModel requestModel = new CreateOrderRequestModel();
            CustomerDetails customerDetail = new CustomerDetails();
            List<Variants> variants = new List<Variants>();
            foreach (var ticketAttributes in command.EventTicketAttributeList)
            {
                var eventTicketAttribute = eventTicketAttributes.Where(w => w.Id == ticketAttributes.Id).FirstOrDefault();
                var eventTicketDetail = allEventTicketDetails.Where(w => w.Id == eventTicketAttribute.EventTicketDetailId).FirstOrDefault(); ;
                var tiqetEventTicketDetail = _tiqetEventTicketDetailMappingRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                var tiqetVariant = _tiqetVariantDetailRepository.Get(tiqetEventTicketDetail.TiqetVariantDetailId);
                Variants variant = new Variants();
                variant.variant_id = Convert.ToInt32(tiqetVariant.VariantId);
                variant.count = ticketAttributes.TotalTickets;
                variants.Add(variant);
            }
            var item = command.EventTicketAttributeList[0];
            requestModel.product_id = Convert.ToInt32(tiqetEventDetail.ProductId);
            requestModel.day = item.VisitDate.ToString("yyyy-MM-dd");
            if (item.TimeSlot != null && item.TimeSlot != "")
            {
                requestModel.timeslot = item.TimeSlot;
            }
            customerDetail.email = _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.CustomerEmail);
            customerDetail.firstname = user.FirstName;
            customerDetail.lastname = user.LastName;
            customerDetail.phone = _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.CustomerPhone);
            requestModel.customer_details = customerDetail;
            requestModel.variants = variants;
            var responseData = await CreateOrderAsync(requestModel);
            var responseJson = Mapper<CreateOrderResponseModel>.MapFromJson(responseData);
            if (responseJson.success)
            {
                commandResults.Success = true;
                commandResults.OrderRefernceId = responseJson.order_reference_id;
                commandResults.PaymentToken = responseJson.payment_confirmation_token;
            }
            else
            {
                commandResults.Success = false;
                commandResults.OrderRefernceId = null;
                commandResults.PaymentToken = null;
            }
            return commandResults;
        }

        public async Task<string> CreateOrderAsync(CreateOrderRequestModel requestModel)
        {
            string token = GetToken(requestModel, _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.PrivateKey));
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    var content = new StringContent(token, Encoding.Default, "text/plain");
                    var response = await httpClient.PostAsync("orders?lang=en", content);
                    responseData = await response.Content.ReadAsStringAsync();
                    return responseData;
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get Tiqets time slots", e));
                return null;
            }
        }

        public static string GetToken(CreateOrderRequestModel requestModel, string privateKey)
        {
            RSAParameters rsaParams;
            using (var tr = new StringReader(privateKey))
            {
                var pemReader = new PemReader(tr);
                var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair == null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                var json = JsonConvert.SerializeObject(requestModel,
            new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
                return Jose.JWT.Encode(json, rsa, Jose.JwsAlgorithm.RS256);
            }
        }
    }
}