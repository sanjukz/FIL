using HubSpot.NET.Core.Interfaces;
using FIL.Api.Events.Event.HubSpot;
using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Hubspot;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FIL.Api.Events.Handlers.HubSpot
{
    public class ScanEventHandler : INotificationHandler<ScanEvent>
    {
        private readonly IHubSpotApi _hubSpotApi;
        private readonly ITransactionRepository _transactionRepository;
        private readonly FIL.Logging.ILogger _logger;

        public ScanEventHandler(IHubSpotApi hubSpotApi, ITransactionRepository transactionRepository, FIL.Logging.ILogger logger)
        {
            _hubSpotApi = hubSpotApi;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public Task Handle(ScanEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Transaction transaction = _transactionRepository.Get(notification.TransactionId);
                if (transaction != null)
                {
                    if (!string.IsNullOrWhiteSpace(transaction.EmailId))
                    {
                        var contact = _hubSpotApi.Contact.GetByEmail<RasvHubspotContact>(transaction.EmailId);
                        if (contact != null)
                        {
                            var dateTimeOffset = new DateTimeOffset(new DateTime(notification.ScanDateTime.Year, notification.ScanDateTime.Month, notification.ScanDateTime.Day, 00, 00, 00, 00, DateTimeKind.Utc));
                            var unixDateTime = dateTimeOffset.ToUnixTimeMilliseconds();

                            contact = _hubSpotApi.Contact.CreateOrUpdate(new RasvHubspotContact
                            {
                                //Id = contact.Id,
                                Email = contact.Email ?? transaction.EmailId,
                                RMS2019DateScannedTicketAtGate = unixDateTime.ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return Task.FromResult(0);
        }
    }
}