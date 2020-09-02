using FIL.Api.Repositories;
using FIL.Contracts.Commands.TournamentLayout;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TournamentLayout
{
    public class EditStandDataCommandHandler : BaseCommandHandlerWithResult<EditStandDataCommand, EditStandDataCommandResult>
    {
        private readonly ITournamentLayoutRepository _tournamentLayoutRepository;
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly ITournamentLayoutSectionRepository _tournamentLayoutSectionRepository;
        private readonly IEntryGateRepository _entryGateRepository;

        public EditStandDataCommandHandler(ITournamentLayoutRepository tournamentLayoutRepository, IEntryGateRepository entryGateRepository, ITournamentLayoutSectionRepository tournamentLayoutSectionRepository, IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository, IMediator mediator) : base(mediator)
        {
            _entryGateRepository = entryGateRepository;
            _tournamentLayoutRepository = tournamentLayoutRepository;
            _tournamentLayoutSectionRepository = tournamentLayoutSectionRepository;
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
        }

        protected override async Task<ICommandResult> Handle(EditStandDataCommand command)
        {
            // var tournamentLayout = _tournamentLayoutRepository.GetByMasterLayoutAndEventId(command.MasterVenueLayoutId,command.EventId).FirstOrDefault();
            var standDetails = _tournamentLayoutSectionRepository.Get(command.SectionId);
            var entryGate = _entryGateRepository.GetByAltId(new Guid(command.EntryGateAltId));
            if (standDetails != null)
            {
                if (standDetails.TournamentLayoutSectionId == 0)
                {
                    standDetails.SectionName = command.StandName;
                    standDetails.Capacity = command.Capacity;
                    standDetails.EntryGateId = entryGate.Id;
                    standDetails.ModifiedBy = command.ModifiedBy;
                    _tournamentLayoutSectionRepository.Save(standDetails);
                    return new EditStandDataCommandResult
                    {
                        Success = true,
                        IsNotCapacityAvailable = false,
                        AvailableCapacity = command.Capacity
                    };
                }
                else
                {
                    var count = 0;
                    var sections = _tournamentLayoutSectionRepository.GetTournamentLayoutSectionId(standDetails.TournamentLayoutSectionId);
                    foreach (var item in sections)
                    {
                        count = count + item.Capacity;
                    }
                    //var existingChildCapacity = _masterVenueLayoutSectionRepository.GetExistingChildCapacityAtTournament(standDetails.TournamentLayoutSectionId);
                    var parentCapacity = _tournamentLayoutSectionRepository.Get(standDetails.TournamentLayoutSectionId).Capacity;
                    var availableCapacity = parentCapacity - count;
                    if (availableCapacity >= command.Capacity)
                    {
                        standDetails.SectionName = command.StandName;
                        standDetails.Capacity = command.Capacity;
                        standDetails.EntryGateId = entryGate.Id;
                        standDetails.ModifiedBy = command.ModifiedBy;
                        _tournamentLayoutSectionRepository.Save(standDetails);
                        return new EditStandDataCommandResult
                        {
                            Success = true,
                            IsNotCapacityAvailable = false,
                            AvailableCapacity = command.Capacity
                        };
                    }
                    else
                    {
                        return new EditStandDataCommandResult
                        {
                            Success = false,
                            IsNotCapacityAvailable = true,
                            AvailableCapacity = availableCapacity
                        };
                    }
                }
            }
            else
            {
                return new EditStandDataCommandResult
                {
                    Success = false,
                    IsNotCapacityAvailable = false,
                    AvailableCapacity = command.Capacity
                };
            }
        }
    }
}