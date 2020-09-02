using FIL.Api.Events.Event.HubSpot;
using FIL.Configuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FIL.Api.Controllers
{
    public class HubspotScanController : Controller
    {
        private readonly ISettings _settings;
        private readonly IMediator _mediator;

        public HubspotScanController(ISettings settings, IMediator mediator)
        {
            _settings = settings;
            _mediator = mediator;
        }

        [Route("hubspotscan/{transactionid}/{scandatetime:datetime?}")]
        public string HubspotScan(string transactionid, DateTime scandatetime)
        {
            _mediator.Publish(new ScanEvent
            {
                TransactionId = Convert.ToInt64(transactionid),
                ScanDateTime = scandatetime
            });
            return "";
        }
    }
}