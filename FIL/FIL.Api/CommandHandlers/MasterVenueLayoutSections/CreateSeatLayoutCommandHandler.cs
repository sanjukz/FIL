using FIL.Api.Repositories;
using FIL.Contracts.Commands.MasterVenueLayoutSections;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.MasterVenueLayoutSections
{
    public class CreateSeatLayoutCommandHandler : BaseCommandHandlerWithResult<CreateSeatLayoutCommand, CreateSeatLayoutCommandResult>
    {
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IMasterVenueLayoutSectionSeatRepository _masterVenueLayoutSectionSeatRepository;

        public CreateSeatLayoutCommandHandler(IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository, IMasterVenueLayoutSectionSeatRepository masterVenueLayoutSectionSeatRepository, IMediator mediator) : base(mediator)
        {
            _masterVenueLayoutSectionSeatRepository = masterVenueLayoutSectionSeatRepository;
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
        }

        protected override async Task<ICommandResult> Handle(CreateSeatLayoutCommand command)
        {
            var masterVenueLayoutSectionInfo = _masterVenueLayoutSectionRepository.Get(command.MasterVenueLayoutSectionSeatDetails[0].MasterVenueLayoutSectionId);
            if (masterVenueLayoutSectionInfo.IsSeatExists == true)
            {
                return new CreateSeatLayoutCommandResult
                {
                    IsExisting = true,
                    Success = false,
                    Id = 0,
                };
            }
            else
            {
                foreach (var item in command.MasterVenueLayoutSectionSeatDetails)
                {
                    var MasterVenueLayoutSectionSeatDetails = new MasterVenueLayoutSectionSeat
                    {
                        AltId = Guid.NewGuid(),
                        SeatTag = item.SeatTag,
                        SeatTypeId = item.SeatTypeId,
                        RowNumber = item.RowNumber,
                        RowOrder = item.RowOrder,
                        ColumnNumber = item.ColumnNumber,
                        ColumnOrder = item.ColumnOrder,
                        MasterVenueLayoutSectionId = item.MasterVenueLayoutSectionId,
                        ModifiedBy = command.ModifiedBy,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        CreatedBy = command.ModifiedBy
                    };
                    _masterVenueLayoutSectionSeatRepository.Save(MasterVenueLayoutSectionSeatDetails);
                }

                masterVenueLayoutSectionInfo.IsSeatExists = true;
                _masterVenueLayoutSectionRepository.Save(masterVenueLayoutSectionInfo);
                return new CreateSeatLayoutCommandResult
                {
                    IsExisting = false,
                    Success = true,
                };
            }
        }
    }
}