using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS.Booking;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS.Handover
{
    public class HandoverDetailsCommandHandler : BaseCommandHandlerWithResult<HandoverDetailsCommand, HandoverDetailsCommandResult>
    {
        private readonly IHandoverSheetRepository _handoverSheetRepository;
        private readonly IUserRepository _userRepository;

        public HandoverDetailsCommandHandler(IHandoverSheetRepository handoverSheetRepository, IUserRepository userRepository, IMediator mediator)
            : base(mediator)
        {
            _handoverSheetRepository = handoverSheetRepository;
            _userRepository = userRepository;
        }

        protected override Task<ICommandResult> Handle(HandoverDetailsCommand command)
        {
            HandoverDetailsCommandResult handoverDetailsCommandResult = new HandoverDetailsCommandResult();
            try
            {
                HandoverSheet handoverSheet = new HandoverSheet();
                var userDetails = _userRepository.GetByAltId(command.ModifiedBy);
                foreach (var item in command.TicketDetails)
                {
                    handoverSheet = _handoverSheetRepository.Save(new HandoverSheet
                    {
                        TransactionId = command.TransactionId,
                        SerialStart = item.SerialStart,
                        SerialEnd = item.SerialEnd,
                        Quantity = item.Quantity,
                        TicketHandedBy = userDetails.FirstName + " " + userDetails.LastName,
                        TicketHandedTo = item.TicketHandedTo,
                        IsEnabled = true,
                        ModifiedBy = command.ModifiedBy
                    });
                }
                handoverDetailsCommandResult.Id = handoverSheet.Id;
                handoverDetailsCommandResult.Success = true;
            }
            catch (Exception ex)
            {
                handoverDetailsCommandResult.Id = -1;
                handoverDetailsCommandResult.Success = true;
                handoverDetailsCommandResult.ErrorMessage = ex.ToString();
            }
            return Task.FromResult<ICommandResult>(handoverDetailsCommandResult);
        }
    }
}