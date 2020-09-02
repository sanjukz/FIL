using FIL.Api.CommandHandlers;
using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class SaveEventImageCommandHandler : BaseCommandHandlerWithResult<EventImageCommand, EventImageCommandResult>
    {
        private readonly IEventGalleryImageRepository _eventGalleryImageRepository;
        private readonly IStepProvider _stepProvider;
        private readonly FIL.Logging.ILogger _logger;

        public SaveEventImageCommandHandler(
            IEventGalleryImageRepository eventGalleryImageRepository,
            IStepProvider stepProvider,
            FIL.Logging.ILogger logger,
            IMediator mediator) : base(mediator)
        {
            _eventGalleryImageRepository = eventGalleryImageRepository;
            _stepProvider = stepProvider;
            _logger = logger;
        }

        protected FIL.Contracts.DataModels.EventGalleryImage SaveEventGallery(EventImageCommand command,
            FIL.Contracts.DataModels.EventGalleryImage eventGalleryImage
            )
        {
            var eventGalleryImageDataModel = new EventGalleryImage
            {
                Id = eventGalleryImage != null ? eventGalleryImage.Id : 0,
                AltId = eventGalleryImage != null ? eventGalleryImage.AltId : Guid.NewGuid(),
                EventId = command.EventImageModel.EventId,
                Name = "NA",
                IsBannerImage = command.EventImageModel.IsBannerImage,
                IsHotTicketImage = command.EventImageModel.IsHotTicketImage,
                IsPortraitImage = command.EventImageModel.IsPortraitImage,
                IsVideoUploaded = command.EventImageModel.IsVideoUploaded,
                IsEnabled = true,
                CreatedUtc = eventGalleryImage != null ? eventGalleryImage.CreatedUtc : DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                CreatedBy = eventGalleryImage != null ? eventGalleryImage.CreatedBy : command.ModifiedBy,
                UpdatedBy = command.ModifiedBy,
                ModifiedBy = command.ModifiedBy
            };
            return _eventGalleryImageRepository.Save(eventGalleryImageDataModel);
        }

        protected override async Task<ICommandResult> Handle(EventImageCommand command)
        {
            try
            {
                var eventGalleryList = _eventGalleryImageRepository.GetByEventId(command.EventImageModel.EventId).FirstOrDefault();
                var savedGalleryImage = SaveEventGallery(command, eventGalleryList);
                command.EventImageModel.Id = savedGalleryImage.Id;
                var eventStepDetail = _stepProvider.SaveEventStepDetails(command.EventImageModel.EventId, command.CurrentStep, true, command.ModifiedBy);
                return new EventImageCommandResult
                {
                    EventImageModel = command.EventImageModel,
                    CompletedStep = eventStepDetail.CompletedStep,
                    CurrentStep = eventStepDetail.CurrentStep,
                    Success = true
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Fail to update the Event Image", e));
                return new EventImageCommandResult { };
            }
        }
    }
}