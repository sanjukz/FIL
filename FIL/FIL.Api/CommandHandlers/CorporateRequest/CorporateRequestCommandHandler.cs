using FIL.Api.Repositories;
using FIL.Contracts.Commands.CorporateRequest;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CorporateRequest
{
    public class CorporateRequestCommandHandler : BaseCommandHandlerWithResult<SaveCorporateRequestDetailsCommand, SaveCorporateRequestDetailsCommandResult>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ICorporateRequestRepository _corporateRequestRepository;
        private readonly ICorporateRequestDetailRepository _corporateRequestDetailRepository;

        public CorporateRequestCommandHandler(ISponsorRepository sponsorsRepository, ICountryRepository countryRepository, ICorporateRequestRepository corporateRequestRepository, ICorporateRequestDetailRepository corporateRequestDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _sponsorRepository = sponsorsRepository;
            _countryRepository = countryRepository;
            _corporateRequestRepository = corporateRequestRepository;
            _corporateRequestDetailRepository = corporateRequestDetailRepository;
        }

        protected override Task<ICommandResult> Handle(SaveCorporateRequestDetailsCommand command)
        {
            SaveCorporateRequestDetailsCommandResult SaveCorporateRequestDetails = new SaveCorporateRequestDetailsCommandResult();
            var country = _countryRepository.GetByAltId(command.PhoneCode);
            var sponsorName = $"{command.FirstName} {command.LastName}";
            var sponsor = _sponsorRepository.GetBySponsorName(sponsorName);
            long sponsorId = 0;
            if (sponsor != null)
            {
                sponsorId = sponsor.Id;
            }
            else
            {
                var sponsorDetails = new FIL.Contracts.DataModels.Sponsor
                {
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    PhoneCode = country.Phonecode.ToString(),
                    PhoneNumber = command.PhoneNumber,
                    Email = command.Email,
                    SponsorName = sponsorName,
                    SponsorTypeId = Contracts.Enums.SponsorType.None,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true
                };
                var sponsorResult = _sponsorRepository.Save(sponsorDetails);
                sponsorId = sponsorResult.Id;
            }
            var corporateRequest = new FIL.Contracts.DataModels.CorporateRequest
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneCode = country.Phonecode.ToString(),
                PhoneNumber = command.PhoneNumber,
                Email = command.Email,
                City = command.City,
                Country = country.Name,
                Company = command.Company,
                SponsorName = sponsorName,
                SponsorId = sponsorId,
                Address = command.Address,
                PickupRepresentativeFirstName = "",
                PickupRepresentativeLastName = "",
                PickupRepresentativeEmail = "",
                PickupRepresentativePhoneCode = "",
                PickupRepresentativePhoneNumber = "",
                ZipCode = command.ZipCode,
                CreatedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                IsEnabled = true
            };
            var result = _corporateRequestRepository.Save(corporateRequest);
            SaveCorporateRequestDetails.CorporateRequest = result;
            SaveCorporateRequestDetails.Success = true;
            return Task.FromResult<ICommandResult>(SaveCorporateRequestDetails);
            // var corporateRequestDetails =new CorporateRequestDetail
            // {
            //     CorporateRequestId=result.Id,
            //     RequestedTickets=command.Seats,
            //     ApprovedTickets=0,
            //     Price=0,
            //     ApprovedStatus=false,
            //     IsEnabled=true,
            //     CreatedUtc=DateTime.UtcNow,
            //     CreatedBy=command.ModifiedBy,
            //     EventTicketAttributeId=command.Stand,
            //     TicketTypeId=0
            // };
            //_corporateRequestDetailRepository.Save(corporateRequestDetails);
        }
    }
}