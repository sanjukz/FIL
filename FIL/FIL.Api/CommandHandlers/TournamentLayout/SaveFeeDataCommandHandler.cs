using FIL.Api.Repositories;
using FIL.Contracts.Commands.TournamentLayout;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TournamentLayout
{
    public class SaveFeeDataCommandHandler : BaseCommandHandlerWithResult<SaveFeeDataCommand, SaveFeeDataCommandResult>
    {
        private readonly IEventFeeTypeMappingRepository _eventFeeTypeMappingRepository;

        public SaveFeeDataCommandHandler(IEventFeeTypeMappingRepository eventFeeTypeMappingRepository, IMediator mediator) : base(mediator)
        {
            _eventFeeTypeMappingRepository = eventFeeTypeMappingRepository;
        }

        protected override async Task<ICommandResult> Handle(SaveFeeDataCommand command)
        {
            var eventfeedata = new EventFeeTypeMapping();
            foreach (var item in command.FeeDetails)
            {
                try
                {
                    if (command.isInsert)
                    {
                        eventfeedata = _eventFeeTypeMappingRepository.Save(new EventFeeTypeMapping
                        {
                            EventId = command.EventId,
                            ChannelId = (Contracts.Enums.Channels)item.channel,
                            FeeId = (Contracts.Enums.FeeType)item.feeType,
                            ValueTypeId = (Contracts.Enums.ValueTypes)item.valueType,
                            Value = item.Value,
                            CreatedUtc = new DateTime(),
                            UpdatedUtc = null,
                            CreatedBy = command.ModifiedBy,
                            UpdatedBy = null,
                            ModifiedBy = command.ModifiedBy
                        });
                    }
                    else
                    {
                        eventfeedata = _eventFeeTypeMappingRepository.Get(item.id);
                        eventfeedata.ChannelId = item.channel.ToString() != null ? (Contracts.Enums.Channels)item.channel : eventfeedata.ChannelId;
                        eventfeedata.FeeId = item.feeType.ToString() != null ? (Contracts.Enums.FeeType)item.feeType : eventfeedata.FeeId;
                        eventfeedata.ValueTypeId = item.valueType.ToString() != null ? (Contracts.Enums.ValueTypes)item.valueType : eventfeedata.ValueTypeId;
                        eventfeedata.Value = item.Value.ToString() != null ? item.Value : eventfeedata.Value;
                        _eventFeeTypeMappingRepository.Save(eventfeedata);
                    }
                }
                catch (Exception ex)
                {
                    return new SaveFeeDataCommandResult
                    {
                        Success = false,
                        Id = 0
                    };
                }
            }

            if (eventfeedata != null)
            {
                return new SaveFeeDataCommandResult
                {
                    Success = true,
                    Id = 0
                };
            }
            else
            {
                return new SaveFeeDataCommandResult
                {
                    Success = false,
                    Id = 0
                };
            }
        }
    }
}