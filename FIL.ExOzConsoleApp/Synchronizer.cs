using Autofac;
using FIL.Configuration;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations.ExOz;
using FIL.Foundation.Senders;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzProductOptionResponse;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzSessionResponse;

namespace FIL.ExOzConsoleApp
{
    public class Synchronizer : ISynchronizer
    {
        private readonly ICommandSender _commandSender;
        private readonly IConsoleLogger _logger;
        private readonly ISettings _settings;
        private readonly ISynchronizer<SaveExOzCountryCommandResult, object> _countrySynchronizer;
        private readonly ISynchronizer<SaveExOzStateCommandResult, string> _stateSynchronizer;
        private readonly ISynchronizer<SaveExOzRegionCommandResult, string> _regionSynchronizer;
        private readonly ISynchronizer<ProductList, OperatorList> _operatorSynchronizer;
        private readonly ISynchronizer<SessionList, ProductList> _productSynchronizer;
        private readonly ISynchronizer<ProductOptionList, SessionList> _productSessionSynchronizer;
        private readonly ISynchronizer<List<ExOzProductOption>, ProductOptionList> _productOptionSynchronizer;

        public Synchronizer(ICommandSender commandSender,
            ISettings settings,
            IConsoleLogger logger,
            ISynchronizer<SaveExOzCountryCommandResult, object> countrySynchronizer,
            ISynchronizer<SaveExOzStateCommandResult, string> stateSynchronizer,
            ISynchronizer<SaveExOzRegionCommandResult, string> regionSynchronizer,
            ISynchronizer<ProductList, OperatorList> operatorSynchronizer,
            ISynchronizer<SessionList, ProductList> productSynchronizer,
            ISynchronizer<ProductOptionList, SessionList> productSessionSynchronizer,
            ISynchronizer<List<ExOzProductOption>, ProductOptionList> productOptionSynchronizer
            )

        {
            _settings = settings;
            _logger = logger;
            _commandSender = commandSender;
            _countrySynchronizer = countrySynchronizer;
            _stateSynchronizer = stateSynchronizer;
            _regionSynchronizer = regionSynchronizer;
            _operatorSynchronizer = operatorSynchronizer;
            _productSynchronizer = productSynchronizer;
            _productSessionSynchronizer = productSessionSynchronizer;
            _productOptionSynchronizer = productOptionSynchronizer;
        }

        public async Task Synchronize()
        {
            await _countrySynchronizer.Synchronize(null);
            await _stateSynchronizer.Synchronize(null);
            SaveExOzRegionCommandResult regionResponse = await _regionSynchronizer.Synchronize(null);
            ProductList productResponse = await _operatorSynchronizer.Synchronize(regionResponse.OperatorList);
            //SessionList sessionResponse = await _productSynchronizer.Synchronize(productResponse);
            //ProductOptionList productOptionResponse = await _productSessionSynchronizer.Synchronize(sessionResponse);
            //await _productOptionSynchronizer.Synchronize(productOptionResponse);

            return;
        }
    }
}