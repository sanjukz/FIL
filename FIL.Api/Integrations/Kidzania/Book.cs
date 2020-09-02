using KidzaniaService;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.Kidzania;
using FIL.Contracts.Models.Integrations.Kidzania.BookCreateOption;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.Kidzania
{
    public interface IBook : IService
    {
        Task<IResponse<BookResponse>> BookAsync(BookOption bookOption);
    }

    public class Book : Service<BookResponse>, IBook
    {
        public Book(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<BookResponse>> BookAsync(BookOption bookOption)
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

                Book_Out bookOut = await ticketBookSoapClient.BookAsync(Convert.ToInt64(parkId), bookOption.ShiftId, bookOption.VisitDate, bookOption.NoOfTickets, visitorTypeId[0], visitorTypeDesc[0], visitorName[0], visitorGender[0], visitorAge[0], visitorTypeId[1], visitorTypeDesc[1], visitorName[1], visitorGender[1], visitorAge[1], visitorTypeId[2], visitorTypeDesc[2], visitorName[2], visitorGender[2], visitorAge[2], visitorTypeId[3], visitorTypeDesc[3], visitorName[3], visitorGender[3], visitorAge[3], visitorTypeId[4], visitorTypeDesc[4], visitorName[4], visitorGender[4], visitorAge[4], visitorTypeId[5], visitorTypeDesc[5], visitorName[5], visitorGender[5], visitorAge[5], visitorTypeId[6], visitorTypeDesc[6], visitorName[6], visitorGender[6], visitorAge[6], visitorTypeId[7], visitorTypeDesc[7], visitorName[7], visitorGender[7], visitorAge[7], visitorTypeId[8], visitorTypeDesc[8], visitorName[8], visitorGender[8], visitorAge[8], visitorTypeId[9], visitorTypeDesc[9], visitorName[9], visitorGender[9], visitorAge[9], partnerId, password, bookOption.TransactionId, bookOption.Ip).ConfigureAwait(false);

                return GetResponse(true, AutoMapper.Mapper.Map<List<BookResponse>>(bookOut).FirstOrDefault());
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to book tickets", ex));
                return GetResponse(false, null);
            }
        }
    }
}