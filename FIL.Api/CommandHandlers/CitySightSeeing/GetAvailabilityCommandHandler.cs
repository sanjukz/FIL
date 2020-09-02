//using Kz.Api.Integrations.HttpHelpers;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.CitySightSeeing;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CitySightSeeing
{
    public class GetAvailabilityCommandHandler : BaseCommandHandlerWithResult<GetAvailabilityCommand, GetAvailabilityCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public GetAvailabilityCommandHandler(ISettings settings, ILogger logger,
        IMediator mediator) : base(mediator)
        {
            _logger = logger;
            _settings = settings;
        }

        protected override async Task<ICommandResult> Handle(GetAvailabilityCommand command)
        {
            GetAvailabilityCommandResult results = new GetAvailabilityCommandResult();
            var availabilityResponse = await FetchAvailability(command.Date, command.TicketId);
            List<string> availableSlots = new List<string>();
            if (availabilityResponse != null && availabilityResponse.data != null)
            {
                foreach (var currentResponse in availabilityResponse.data.availabilities.Where(s => s.vacancies != " 0"))
                {
                    availableSlots.Add(currentResponse.from_date_time.TimeOfDay.ToString());
                }
                results.AvailableSlots = availableSlots;
            }
            return results;
        }

        private async Task<AvailabilityResponse> FetchAvailability(string date, string ticketId)
        {
            try
            {
                AvailabilityRequestModel input = new AvailabilityRequestModel();
                input.data = new Data();
                input.request_type = "availabilities";
                input.data.distributor_id = _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.DistributorId);
                AvailabilityResponse objData = new AvailabilityResponse();
                input.data.from_date = date;
                input.data.to_date = date;
                input.data.ticket_id = ticketId;
                objData = Mapper<AvailabilityResponse>.MapFromJson((await Get(input)));
                return objData;
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to Fetch CitySightSeeing Avaikability", ex));
                return null;
            }
        }

        public async Task<string> Get(object obj)
        {
            var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.Endpoint));

            string responseData;
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.Timeout = new TimeSpan(1, 0, 0);

                string auth = string.Format(_settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestIdentifier) + ":" + _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.Token));

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-request-authentication", _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestAuthentication));

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-request-identifier", _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestIdentifier));

                var json = JsonConvert.SerializeObject(obj);

                using (var content = new StringContent(json, System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("booking_service", content))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return responseData;
        }
    }
}