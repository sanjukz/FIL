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
    public interface IVisitorType : IService
    {
        Task<IResponse<VisitorTypeResponse>> GetVisitorTypes(DateTime visitDate, long ShiftId);
    }

    public class VisitorType : Service<VisitorTypeResponse>, IVisitorType
    {
        public VisitorType(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<VisitorTypeResponse>> GetVisitorTypes(DateTime visitDate, long ShiftId)
        {
            try
            {
                var partnerId = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.PartnerId);
                var password = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.Password);
                var cityId = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.CityId);
                var parkId = _settings.GetConfigSetting<string>(SettingKeys.Integration.KidZania.ParkId);

                TicketBookSoapClient ticketBookSoapClient = new TicketBookSoapClient(TicketBookSoapClient.EndpointConfiguration.TicketBookSoap);
                Vd_Park_Out[] parkOuts = await ticketBookSoapClient.FnSchedule_VisitDate_ParkAsync(Convert.ToInt64(parkId), visitDate, partnerId, password).ConfigureAwait(false);
                List<VisitorTypes> VisitorTypes = new List<VisitorTypes>();

                if (ShiftId != 0)
                {
                    IEnumerable<Vd_Park_Out> parkOutsByShiftId = parkOuts.Where(p => p.ShiftId == ShiftId);
                    if (parkOutsByShiftId != null)
                    {
                        parkOuts = parkOuts.Where(p => p.ShiftId == ShiftId).ToArray();
                    }
                    VisitorTypes = parkOuts[0].VisitorTypes.ToList();
                }

                return GetResponse(true,
                    new VisitorTypeResponse
                    {
                        VisitorTypes = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Integrations.Kidzania.VisitorType>>(VisitorTypes)
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