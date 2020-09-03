using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.Commands.HubSpot;
using FIL.Foundation.Senders;
using Scheduler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Window.Service
{    
    public class HubspotAbandonCart : ISynchronizer
    {
        private readonly ICommandSender _commandSender;        

        public HubspotAbandonCart(ICommandSender commandSender)
        {
            _commandSender = commandSender;         
        }

        public async Task Synchronize()
        {
            await _commandSender.Send(new AbandonCartCommand());
            return;
        }        
    }
}
