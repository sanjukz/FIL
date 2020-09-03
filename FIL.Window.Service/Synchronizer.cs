using Autofac;
using FIL.Configuration;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.Commands.NewsLetterSignUp;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations.ExOz;
using FIL.Foundation.Senders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scheduler;
using FIL.Contracts.Commands.CitySightSeeing;

namespace FIL.Window.Service
{
    public class Synchronizer : ISynchronizer
    {
        private readonly ICommandSender _commandSender;
        private readonly ISettings _settings;
        private readonly ISynchronizer _hubspotSynchronizer;

        public Synchronizer(ICommandSender commandSender,
        ISettings settings, ISynchronizer hubspotSynchronizer, ISynchronizer hohoDataSynchronizer, ISynchronizer valueRetailDataSynchronizer
        )
        {
            _settings = settings;
            _commandSender = commandSender;
            _hubspotSynchronizer = hubspotSynchronizer;
        }

        public async Task Synchronize()
        {
            await _hubspotSynchronizer.Synchronize();
            return;
        }
    }
}
