using FIL.Api.Repositories;
using FIL.Contracts.Commands.MasterVenueLayoutSections;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.MasterVenueLayoutSections
{
    public class CreateNewEntryGateCommandHandler : BaseCommandHandlerWithResult<CreateNewEntryGateCommand, CreateNewEntryGateCommandResult>
    {
        private readonly IEntryGateRepository _entryGateRepository;

        public CreateNewEntryGateCommandHandler(IEntryGateRepository entryGateRepository, IMediator mediator) : base(mediator)
        {
            _entryGateRepository = entryGateRepository;
        }

        protected override async Task<ICommandResult> Handle(CreateNewEntryGateCommand command)
        {
            var entryGate = _entryGateRepository.GetByName(command.NewEntryGatename);
            if (entryGate == null)
            {
                var EntryGate = new EntryGate();
                var details = new EntryGate
                {
                    AltId = new Guid(),
                    Name = command.NewEntryGatename,
                    StreetInformation = "",
                    IsEnabled = true,
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                };
                EntryGate = _entryGateRepository.Save(details);
                return new CreateNewEntryGateCommandResult
                {
                    IsExisting = false,
                    AltId = EntryGate.AltId.ToString(),
                    Id = EntryGate.Id,
                    Success = true
                };
            }
            else
            {
                return new CreateNewEntryGateCommandResult
                {
                    IsExisting = true,
                    AltId = entryGate.AltId.ToString(),
                    Id = entryGate.Id,
                    Success = false
                };
            }
        }
    }
}