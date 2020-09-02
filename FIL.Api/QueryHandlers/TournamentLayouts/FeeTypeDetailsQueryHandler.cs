using FIL.Contracts.Enums;
using FIL.Contracts.Queries.TournamentLayout;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.TournamentLayouts
{
    public class FeeTypeDetailsQueryHandler : IQueryHandler<FeeTypeQuery, FeeTypeQueryResult>
    {
        private readonly FIL.Logging.ILogger _logger;

        public FeeTypeDetailsQueryHandler(FIL.Logging.ILogger logger)
        {
            _logger = logger;
        }

        public FeeTypeQueryResult Handle(FeeTypeQuery query)
        {
            try
            {
                var feetype = Enum.GetValues(typeof(FeeType));
                var channel = Enum.GetValues(typeof(Channels));
                var valuetype = Enum.GetValues(typeof(ValueTypes));
                List<FIL.Contracts.Models.Channel> channelList = new List<FIL.Contracts.Models.Channel>();
                List<FIL.Contracts.Models.FeeType> feeList = new List<FIL.Contracts.Models.FeeType>();
                List<FIL.Contracts.Models.ValueType> valueList = new List<FIL.Contracts.Models.ValueType>();
                foreach (var item in feetype)
                {
                    FIL.Contracts.Models.FeeType fee = new Contracts.Models.FeeType();
                    fee.Id = (int)item;
                    fee.name = item.ToString();
                    feeList.Add(fee);
                }
                foreach (var item in channel)
                {
                    FIL.Contracts.Models.Channel channels = new Contracts.Models.Channel();
                    channels.Id = (int)item;
                    channels.name = item.ToString();
                    channelList.Add(channels);
                }
                foreach (var item in valuetype)
                {
                    FIL.Contracts.Models.ValueType values = new Contracts.Models.ValueType();
                    values.Id = (int)item;
                    values.name = item.ToString();
                    valueList.Add(values);
                }
                return new FeeTypeQueryResult
                {
                    FeeType = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.FeeType>>(feeList),
                    Channels = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Channel>>(channelList),
                    ValueType = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.ValueType>>(valueList)
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new FeeTypeQueryResult
                {
                    FeeType = null,
                    Channels = null,
                    ValueType = null
                };
            }
        }
    }
}