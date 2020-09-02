using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class SaveAmenityCommandHandler : BaseCommandHandlerWithResult<SaveAmenityCommand, SaveAmenityCommandResult>
    {
        private readonly IAmenityRepository _amenityRepository;

        public SaveAmenityCommandHandler(IAmenityRepository amenityRepository, IMediator mediator

        )
            : base(mediator)
        {
            _amenityRepository = amenityRepository;
        }

        protected override Task<ICommandResult> Handle(SaveAmenityCommand command)
        {
            var amenity = new Amenities();
            //id, altid, eventcategoryid, eventtyhpeid, name, clientpointofcontactid, termsandconditions,
            //isenabled,createdutc, createdby, eventsourceid,
            SaveAmenityCommandResult saveEventDataResult = new SaveAmenityCommandResult();
            amenity.Id = 0;
            amenity.Amenity = command.Amenity;
            try
            {
                FIL.Contracts.DataModels.Amenities result = _amenityRepository.SaveAmenity(amenity);

                saveEventDataResult.Id = result.Id;
            }
            catch (Exception ex)
            {
            }
            return Task.FromResult<ICommandResult>(saveEventDataResult);
        }
    }
}