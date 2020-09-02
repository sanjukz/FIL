using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations.ExOz;
using FIL.Foundation.Senders;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzProductOptionResponse;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzSessionResponse;

namespace FIL.ExOzConsoleApp
{
    public class SyncExOzProductOptions : ISynchronizer<List<ExOzProductOption>, ProductOptionList>
    {
        private readonly ICommandSender _commandSender;
        private readonly IConsoleLogger _logger;

        public SyncExOzProductOptions(ICommandSender commandSender, IConsoleLogger logger)
        {
            _commandSender = commandSender;
            _logger = logger;

        }

        public async Task<List<ExOzProductOption>> Synchronize(ProductOptionList productOptionDetails)
        {
            _logger.StartMsg("Options");

            //Insert Options Here
            try
            {
                SaveExOzProductOptionCommandResult retOptions = await _commandSender.Send<SaveExOzProductOptionCommand, SaveExOzProductOptionCommandResult>(new SaveExOzProductOptionCommand
                {
                    OptionList = productOptionDetails.ProductOptions,
                    ModifiedBy = new Guid("C043DDEE-D0B1-48D8-9C3F-309A77F44793")
                });
                _logger.FinishMsg(retOptions.OptionList.Count, "Options");
                return retOptions.OptionList;
            }
            catch (Exception e)
            {
                _logger.Log($"Exception: {e.Message}");
                throw;
            }
        }
    }
}
