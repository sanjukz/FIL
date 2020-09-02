using KidzaniaService;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.Kidzania;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.Kidzania
{
    public interface IShift : IService
    {
        Task<IResponse<ShiftResponse>> GetShifts(DateTime visitDate);
    }

    public class Shift : Service<ShiftResponse>, IShift
    {
        public Shift(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<ShiftResponse>> GetShifts(DateTime visitDate)
        {
            try
            {
                var partnerId = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.PartnerId);
                var password = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.Password);
                var cityId = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.CityId);
                var parkId = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.ParkId);

                TicketBookSoapClient ticketBookSoapClient = new TicketBookSoapClient(TicketBookSoapClient.EndpointConfiguration.TicketBookSoap);
                Shift_Out[] shiftOut = await ticketBookSoapClient.FnSchedule_ShiftAsync(Convert.ToInt64(cityId), Convert.ToInt64(parkId), visitDate, partnerId, password).ConfigureAwait(false);
                Vd_Park_Out[] parkOut = await ticketBookSoapClient.FnSchedule_VisitDate_ParkAsync(Convert.ToInt64(parkId), visitDate, partnerId, password).ConfigureAwait(false);

                return GetResponse(true,
                    new ShiftResponse
                    {
                        ShiftOut = AutoMapper.Mapper.Map<List<ShiftOut>>(shiftOut),
                        ParkOut = AutoMapper.Mapper.Map<List<ParkOut>>(parkOut)
                    });
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get shifts", ex));
                return GetResponse(false, null);
            }
        }
    }
}