using KidzaniaService;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.Kidzania;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.Kidzania
{
    public interface IBuy : IService
    {
        Task<IResponse<BookResponse>> BuyAsync(BuyOption buyOption);
    }

    public class Buy : Service<BookResponse>, IBuy
    {
        public Buy(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<BookResponse>> BuyAsync(BuyOption buyOption)
        {
            try
            {
                var partnerId = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.PartnerId);
                var password = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.Password);
                var cityId = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.CityId);
                var parkId = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.ParkId);

                TicketBookSoapClient ticketBookSoapClient = new TicketBookSoapClient(TicketBookSoapClient.EndpointConfiguration.TicketBookSoap);
                long[] visitorTypeId = new long[10];
                string[] visitorTypeDesc = new string[10];
                string[] visitorName = new string[10];
                string[] visitorGender = new string[10];
                long[] visitorAge = new long[10];

                for (int i = 0; i < 10; i++)
                {
                    visitorTypeDesc[i] = visitorName[i] = visitorGender[i] = string.Empty;
                }

                Buy_Out buyOut = await ticketBookSoapClient.BuyAsync(Convert.ToInt64(parkId), partnerId, password, Convert.ToInt64(buyOption.TransactionId), buyOption.PayType, buyOption.PayConfNo, buyOption.Remarks, buyOption.MobileNo, buyOption.VisitDate, buyOption.ShiftId, buyOption.OrderId, buyOption.OrderRef, buyOption.MTR, buyOption.TransactionId, buyOption.RRN, buyOption.PayAuthId).ConfigureAwait(false);

                return GetResponse(true, AutoMapper.Mapper.Map<List<BookResponse>>(buyOut).FirstOrDefault());
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to book tickets", ex));
                return GetResponse(false, null);
            }
        }
    }
}