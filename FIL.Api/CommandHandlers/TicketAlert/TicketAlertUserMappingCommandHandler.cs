using FIL.Api.Integrations;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.TicketAlert;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TicketAlert
{
    public class TicketAlertUserMappingCommandHandler : BaseCommandHandler<TicketAlertUserMappingCommand>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITicketAlertEventMappingRepository _ticketAlertEventMappingRepository;
        private readonly ITicketAlertUserMappingRepository _ticketAlertUserMappingRepository;
        private readonly IIPDetailRepository _iPDetailRepository;
        private readonly IIpApi _ipApi;

        public TicketAlertUserMappingCommandHandler(ICountryRepository countryRepository,
            IEventRepository eventRepository,
            ITicketAlertEventMappingRepository ticketAlertEventMappingRepository,
            ITicketAlertUserMappingRepository ticketAlertUserMappingRepository,
            IIPDetailRepository iPDetailRepository, IIpApi ipApi,
            IMediator mediator)
           : base(mediator)
        {
            _countryRepository = countryRepository;
            _eventRepository = eventRepository;
            _ticketAlertEventMappingRepository = ticketAlertEventMappingRepository;
            _ticketAlertUserMappingRepository = ticketAlertUserMappingRepository;
            _iPDetailRepository = iPDetailRepository;
            _ipApi = ipApi;
        }

        protected override async Task Handle(TicketAlertUserMappingCommand command)
        {
            IPDetail ipDetail = new IPDetail();
            if (!string.IsNullOrWhiteSpace(command.Ip))
            {
                ipDetail = _iPDetailRepository.GetByIpAddress(command.Ip);
                if (ipDetail == null)
                {
                    IResponse<Contracts.Models.Integrations.IpApiResponse> ipApiResponse = _ipApi.GetUserLocationByIp(command.Ip).Result;
                    if (ipApiResponse.Result != null)
                    {
                        ipDetail = _iPDetailRepository.Save(new IPDetail
                        {
                            IPAddress = command.Ip,
                            CountryCode = ipApiResponse.Result.CountryCode,
                            CountryName = ipApiResponse.Result.Country,
                            RegionCode = ipApiResponse.Result.Region,
                            RegionName = ipApiResponse.Result.RegionName,
                            City = ipApiResponse.Result.City,
                            Zipcode = ipApiResponse.Result.ZipCode,
                            TimeZone = ipApiResponse.Result.Timezone,
                            Latitude = Convert.ToDecimal(ipApiResponse.Result.Latitude),
                            Longitude = Convert.ToDecimal(ipApiResponse.Result.Longitude)
                        });
                    }
                }
            }

            if ((bool)command.IsStreamingEvent)
            {
                var Data = new TicketAlertUserMapping
                {
                    AltId = Guid.NewGuid(),
                    TicketAlertEventMappingId = 1,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    PhoneCode = command.PhoneCode,
                    PhoneNumber = command.PhoneNumber,
                    Email = command.Email,
                    EventName = command.EventName,
                    TourTravelPackage = command.TourTravelPackages,
                    NumberOfTickets = command.NumberOfTickets,
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true,
                    IPDetailId = ipDetail.Id
                };
                _ticketAlertUserMappingRepository.Save(Data);
            }
            else
            {
                var i = -1;
                foreach (var ticketAlertId in command.TicketAlertEvents)
                {
                    i = i + 1;

                    var Data = new TicketAlertUserMapping
                    {
                        AltId = Guid.NewGuid(),
                        TicketAlertEventMappingId = ticketAlertId,
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        PhoneCode = command.PhoneCode,
                        PhoneNumber = command.PhoneNumber,
                        Email = command.Email,
                        TourTravelPackage = command.TourTravelPackages,
                        NumberOfTickets = command.NumberOfTickets,
                        ModifiedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        IsEnabled = true,
                        IPDetailId = ipDetail.Id
                    };
                    _ticketAlertUserMappingRepository.Save(Data);
                }
            }
        }
    }
}