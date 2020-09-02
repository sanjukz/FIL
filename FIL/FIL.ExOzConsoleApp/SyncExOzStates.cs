using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations.ExOz;
using FIL.Foundation.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIL.ExOzConsoleApp.Entities.Classes;

namespace FIL.ExOzConsoleApp
{
    public class SyncExOzStates : ISynchronizer<SaveExOzStateCommandResult, string>
    {
        private readonly ICommandSender _commandSender;
        private readonly IConsoleLogger _logger;

        public SyncExOzStates(ICommandSender commandSender,
            IConsoleLogger logger)
        {
            _commandSender = commandSender;
            _logger = logger;
        }
        
        public async Task<SaveExOzStateCommandResult> Synchronize(string obj)
        {
            _logger.StartMsg("States");
            var postResponse = HttpWebRequestHelper.ExOz_WebRequestGet("");
            Contracts.Models.Integrations.ExOz.StateResponseList exOzStateResponse = Mapper<Contracts.Models.Integrations.ExOz.StateResponseList>.MapFromJson(postResponse);
            var stateList = exOzStateResponse.states;
            //Code to insert States
            SaveExOzStateCommandResult retStates = await _commandSender.Send<SaveExOzStateCommand, SaveExOzStateCommandResult>
                (new SaveExOzStateCommand
            {
                 StateList = stateList
            });
            _logger.FinishMsg(retStates.StateList.Count(), "States");
            //ExOzRegionList exOzRegionResponse = Mapper<ExOzRegionList>.MapFromJson(obj);
            //return exOzRegionResponse;
            return retStates;
        }
    }
}
