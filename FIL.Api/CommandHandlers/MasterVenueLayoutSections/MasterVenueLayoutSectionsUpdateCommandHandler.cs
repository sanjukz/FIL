using FIL.Api.Repositories;
using FIL.Contracts.Commands.MasterVenueLayoutSections;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsUpdateCommandHandler : BaseCommandHandler<MasterVenueLayoutSectionsUpdateCommand>
    {
        private readonly IMasterVenueLayoutRepository _masterVenueLayoutsRepository;
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IEntryGateRepository _entryGateRepository;

        public MasterVenueLayoutSectionsUpdateCommandHandler(IMasterVenueLayoutRepository masterVenueLayoutRepository, IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository, IEntryGateRepository entryGateRepository, IMediator mediator) : base(mediator)
        {
            _masterVenueLayoutsRepository = masterVenueLayoutRepository;
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _entryGateRepository = entryGateRepository;
        }

        protected override async Task Handle(MasterVenueLayoutSectionsUpdateCommand command)
        {
            var masterVenueLayoutSection = _masterVenueLayoutSectionRepository.GetByAltId(new Guid(command.AltId));

            if (masterVenueLayoutSection != null)
            {
                masterVenueLayoutSection.SectionName = command.SectionName;
                masterVenueLayoutSection.Capacity = command.Capacity;
                masterVenueLayoutSection.EntryGateId = command.EntryGateId;
                masterVenueLayoutSection.ModifiedBy = command.ModifiedBy;
                _masterVenueLayoutSectionRepository.Save(masterVenueLayoutSection);
            }
        }
    }
}