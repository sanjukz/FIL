using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Tiqets;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Tiqets
{
    public class GetTimeSlotsCommandHandler : BaseCommandHandlerWithResult<GetTimeSlotsCommand, GetTimeSlotsCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public GetTimeSlotsCommandHandler(
       ILogger logger, ISettings settings,
        IMediator mediator) : base(mediator)
        {
            _logger = logger;
            _settings = settings;
        }

        protected override async Task<ICommandResult> Handle(GetTimeSlotsCommand command)
        {
            var getSlots = await GetSlots(command);
            GetTimeSlotsCommandResult getTimeSlotsCommandResult = new GetTimeSlotsCommandResult();
            getTimeSlotsCommandResult.TimeSlots = getSlots.Where(s => s.is_available = true).ToList();
            return getTimeSlotsCommandResult;
        }

        public async Task<List<Timeslot>> GetSlots(GetTimeSlotsCommand command)
        {
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    using (var response = await httpClient.GetAsync("products/" + command.ProductId + "/timeslots?lang=en&day=" + command.Day))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
                var responseJson = Mapper<TimeSlotResponseModel>.MapFromJson(responseData);
                return responseJson.timeslots;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get Tiqets time slots", e));
                return null;
            }
        }
    }
}