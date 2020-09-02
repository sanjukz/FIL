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
    public class SyncExOzCountries : ISynchronizer<SaveExOzCountryCommandResult, object>
    {
        private readonly ICommandSender _commandSender;
        private readonly IConsoleLogger _logger;

        public SyncExOzCountries(ICommandSender commandSender,
            IConsoleLogger logger)
        {
            _commandSender = commandSender;
            _logger = logger;
        }

        //public async Task<SaveExOzCountryCommandResult> Synchronize(object obj)
        public async Task<SaveExOzCountryCommandResult> Synchronize(object obj)
        {
            SaveExOzCountryCommandResult retResult = await UpdateCountries();
            
            return retResult;
        }

        public async Task<SaveExOzCountryCommandResult> UpdateCountries()
        {
            _logger.StartMsg("Countries");

            var postResponse = HttpWebRequestHelper.ExOz_WebRequestGet("");
            Contracts.Models.Integrations.ExOz.StateResponseList exOzStateResponse = Mapper<Contracts.Models.Integrations.ExOz.StateResponseList>.MapFromJson(postResponse);
            var countryList = exOzStateResponse.states.Select(m => m.Country).Distinct();
            //Code to insert countries

            try
            {
                var retCountries = await _commandSender.Send<SaveExOzCountryCommand, SaveExOzCountryCommandResult>
                    (new SaveExOzCountryCommand
                    {
                        Names = countryList.ToList()
                    });

                _logger.FinishMsg(retCountries.CountryList.Count(), "Countries");

                return retCountries;
            }
            catch (Exception e)
            {
                _logger.Log($"Exception: {e.Message}");
                throw;
            }
        }
    }
}
