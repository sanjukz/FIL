using FIL.Api.Repositories;
using FIL.Contracts.Commands.MasterVenueLayoutSections;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsCommandHandler : BaseCommandHandler<MasterVenueLayoutSectionsCommand>
    {
        private readonly IMasterVenueLayoutRepository _masterVenueLayoutsRepository;
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IEntryGateRepository _entryGateRepository;

        public MasterVenueLayoutSectionsCommandHandler(IMasterVenueLayoutRepository masterVenueLayoutRepository, IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository, IEntryGateRepository entryGateRepository, IMediator mediator) : base(mediator)
        {
            _masterVenueLayoutsRepository = masterVenueLayoutRepository;
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _entryGateRepository = entryGateRepository;
        }

        protected override async Task Handle(MasterVenueLayoutSectionsCommand command)
        {
            var masterVenueLayout = new MasterVenueLayoutSection
            {
                AltId = Guid.NewGuid(),
                SectionName = command.SectionName,
                Capacity = command.Capacity,
                MasterVenueLayoutId = command.MasterVenueLayoutId,
                MasterVenueLayoutSectionId = command.MasterVenueLayoutSectionId,
                VenueLayoutAreaId = command.VenueLayoutAreaId,
                EntryGateId = command.EntryGateId,
                ModifiedBy = command.ModifiedBy,
                IsEnabled = true
            };
            _masterVenueLayoutSectionRepository.Save(masterVenueLayout);
            return;
        }
    }
}