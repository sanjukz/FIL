using FIL.Api.Providers.EventManagement;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CreateEventV1
{
    public class SaveEventRecurrenceCommandHandler : BaseCommandHandlerWithResult<EventRecurranceCommand, EventRecurranceCommandResult>
    {
        private readonly ISaveScheduleDetailProvider _saveScheduleDetailProvider;
        private readonly IUpdateScheduleDetailProvider _updateScheduleDetailProvider;
        private readonly IDeleteScheduleDetailProvider _deleteScheduleDetailProvider;
        private readonly IScheduleDetailRepository _scheduleDetailRepository;
        private readonly IGetScheduleDetailProvider _getScheduleDetailProvider;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly ILogger _logger;

        public SaveEventRecurrenceCommandHandler(
              ISaveScheduleDetailProvider saveScheduleDetailProvider,
              IScheduleDetailRepository scheduleDetailRepository,
              IUpdateScheduleDetailProvider updateScheduleDetailProvider,
              IDeleteScheduleDetailProvider deleteScheduleDetailProvider,
              IEventDetailRepository eventDetailRepository,
            IGetScheduleDetailProvider getScheduleDetailProvider,
            IEventAttributeRepository eventAttributeRepository,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _logger = logger;
            _saveScheduleDetailProvider = saveScheduleDetailProvider;
            _updateScheduleDetailProvider = updateScheduleDetailProvider;
            _scheduleDetailRepository = scheduleDetailRepository;
            _deleteScheduleDetailProvider = deleteScheduleDetailProvider;
            _getScheduleDetailProvider = getScheduleDetailProvider;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
        }

        protected void UpdateEventDetail(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand)
        {
            var scheduleDetails = _getScheduleDetailProvider.GetScheduleDetails(eventRecurranceCommand.EventId, DateTime.UtcNow, DateTime.UtcNow, true).FirstOrDefault();
            var eventDetail = _eventDetailRepository.GetByEventId(eventRecurranceCommand.EventId);
            eventDetail.StartDateTime = scheduleDetails.StartDateTime;
            eventDetail.EndDateTime = scheduleDetails.EndDateTime;
            eventDetail.IsEnabled = true;
            _eventDetailRepository.Save(eventDetail);
        }

        protected void UpdateEventAttribute(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand)
        {
            EventAttribute eventAttribute = new EventAttribute();
            var eventDetail = _eventDetailRepository.GetByEventId(eventRecurranceCommand.EventId);
            var eventTicketAttribute = _eventAttributeRepository.GetByEventDetailId(eventDetail.Id);
            if (eventTicketAttribute != null)
            {
                _eventAttributeRepository.Delete(eventTicketAttribute);
            }
            var eventAttributes = new EventAttribute
            {
                TimeZoneAbbreviation = eventRecurranceCommand.TimeZoneAbbrivation,
                EventDetailId = eventDetail.Id,
                CreatedBy = eventRecurranceCommand.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                IsEnabled = true,
                TimeZone = eventRecurranceCommand.TimeZoneOffSet,
                ModifiedBy = eventRecurranceCommand.ModifiedBy,
                UpdatedBy = eventRecurranceCommand.ModifiedBy,
                UpdatedUtc = DateTime.UtcNow
            };
            _eventAttributeRepository.Save(eventAttributes);
        }

        protected override async Task<ICommandResult> Handle(EventRecurranceCommand command)
        {
            try
            {
                /* If action is bulk insert */
                if (command.ActionType == ActionType.BulkInsert)
                {
                    var scheduleDetails = _saveScheduleDetailProvider.GetScheduleDetails(command);
                    try
                    {
                        /* Use dapper plus */
                        _scheduleDetailRepository.SaveAll(scheduleDetails);
                    }
                    catch (Exception e)
                    {
                        /* use dapper execute */
                        _scheduleDetailRepository.ExecuteCommand(FIL.Contracts.Utils.Constant.SqlStatement.ScheduleDetail.InsertCommand, scheduleDetails);
                    }
                    UpdateEventDetail(command);
                    UpdateEventAttribute(command);
                }
                /* If action is bulk reschedule or single reschedule */
                else if (command.ActionType == ActionType.BulkReschedule || command.ActionType == ActionType.SingleReschedule)
                {
                    var scheduleDetails = _updateScheduleDetailProvider.GetScheduleDetails(command);
                    try
                    {
                        /* Use dapper plus */
                        _scheduleDetailRepository.SaveAll(scheduleDetails);
                    }
                    catch (Exception e)
                    {
                        /* use dapper execute */
                        _scheduleDetailRepository.ExecuteCommand(FIL.Contracts.Utils.Constant.SqlStatement.ScheduleDetail.UpdateCommand, scheduleDetails);
                    }
                }
                /* If action is delete bulk or single delete */
                else if (command.ActionType == ActionType.BulkDelete || command.ActionType == ActionType.SingleDelete)
                {
                    var scheduleDetails = _deleteScheduleDetailProvider.GetScheduleDetails(command);
                    try
                    {
                        /* Use dapper plus */
                        _scheduleDetailRepository.DeleteAll(scheduleDetails);
                    }
                    catch (Exception e)
                    {
                        /* use dapper execute */
                        _scheduleDetailRepository.ExecuteCommand(FIL.Contracts.Utils.Constant.SqlStatement.ScheduleDetail.DeleteCommand, scheduleDetails);
                    }

                    if (command.ActionType == ActionType.BulkDelete)
                    {
                        var currentScheduleDetails = _scheduleDetailRepository.GetAllByEventScheduleId(command.EventScheduleId);
                        if (!currentScheduleDetails.Any())
                        {
                            _deleteScheduleDetailProvider.DeleteEventSchedule(command);
                        }
                    }
                }
                return new EventRecurranceCommandResult
                {
                    Success = true
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new EventRecurranceCommandResult { };
            }
        }
    }
}