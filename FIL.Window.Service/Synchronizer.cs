using Autofac;
using Kz.Configuration;
using Kz.Contracts.Commands.ExOz;
using Kz.Contracts.Commands.NewsLetterSignUp;
using Kz.Contracts.DataModels;
using Kz.Contracts.Models.Integrations.ExOz;
using Kz.Foundation.Senders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scheduler;
using Kz.Contracts.Commands.CitySightSeeing;

namespace Kz.Window.Service
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
