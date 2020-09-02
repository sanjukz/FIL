using FIL.Api.CommandHandlers;
using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class SaveSponsorCommandHandler : BaseCommandHandlerWithResult<SaveSponsorCommand, SaveSponsorCommandResult>
    {
        private readonly IStepProvider _stepProvider;
        private readonly IFILSponsorDetailRepository _fILSponsorDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public SaveSponsorCommandHandler(
            IStepProvider stepProvider,
            FIL.Logging.ILogger logger,
            IFILSponsorDetailRepository fILSponsorDetailRepository,
            IMediator mediator) : base(mediator)
        {
            _stepProvider = stepProvider;
            _fILSponsorDetailRepository = fILSponsorDetailRepository;
            _logger = logger;
        }

        protected FIL.Contracts.DataModels.FILSponsorDetail SaveEventSponsor(SaveSponsorCommand command,
           FILSponsorDetail newSponsorDetail
           )
        {
            var sponsorData = new FILSponsorDetail
            {
                Id = newSponsorDetail.Id,
                AltId = newSponsorDetail.Id == 0 ? Guid.NewGuid() : newSponsorDetail.AltId,
                Description = newSponsorDetail.Description,
                EventId = command.EventId,
                Name = newSponsorDetail.Name,
                Link = newSponsorDetail.Link,
                Priority = newSponsorDetail.Priority,
                IsEnabled = newSponsorDetail.IsEnabled,
                CreatedUtc = newSponsorDetail.Id != 0 ? newSponsorDetail.CreatedUtc : DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                CreatedBy = newSponsorDetail.Id != 0 ? newSponsorDetail.CreatedBy : command.ModifiedBy,
                UpdatedBy = command.ModifiedBy,
                ModifiedBy = command.ModifiedBy
            };
            return _fILSponsorDetailRepository.Save(sponsorData); ;
        }

        protected override async Task<ICommandResult> Handle(SaveSponsorCommand command)
        {
            try
            {
                var sponsorDetailsList = _fILSponsorDetailRepository.GetAllByEventId(command.EventId).ToList();
                List<FIL.Contracts.DataModels.FILSponsorDetail> sponsorDetails1 = new List<FILSponsorDetail>();
                foreach (FIL.Contracts.DataModels.FILSponsorDetail currentSponsor in command.SponsorDetail)
                {
                    if (sponsorDetailsList.Where(s => s.Id == currentSponsor.Id).Any())
                    {
                        currentSponsor.Id = sponsorDetailsList.Where(s => s.Id == currentSponsor.Id).FirstOrDefault().Id;
                        currentSponsor.AltId = sponsorDetailsList.Where(s => s.Id == currentSponsor.Id).FirstOrDefault().AltId;
                        currentSponsor.CreatedUtc = sponsorDetailsList.Where(s => s.Id == currentSponsor.Id).FirstOrDefault().CreatedUtc;
                        currentSponsor.CreatedBy = sponsorDetailsList.Where(s => s.Id == currentSponsor.Id).FirstOrDefault().CreatedBy;
                        currentSponsor.ModifiedBy = sponsorDetailsList.Where(s => s.Id == currentSponsor.Id).FirstOrDefault().ModifiedBy;
                    }
                    var currentHost = SaveEventSponsor(command, currentSponsor);
                    sponsorDetails1.Add(currentHost);
                }
                var eventStepDetail = _stepProvider.SaveEventStepDetails(command.EventId, command.CurrentStep, true, command.ModifiedBy);
                return new SaveSponsorCommandResult
                {
                    SponsorDetail = sponsorDetails1,
                    CompletedStep = eventStepDetail.CompletedStep,
                    CurrentStep = eventStepDetail.CurrentStep,
                    Success = true
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Fail to update the Event Sponsor", e));
                return new SaveSponsorCommandResult { };
            }
        }
    }
}