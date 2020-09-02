using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.Models.Integrations.ExOz;
using FIL.Foundation.Senders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FIL.ExOzConsoleApp.Entities.Classes;
using FIL.Contracts.DataModels;

namespace FIL.ExOzConsoleApp
{
    public class SyncExOzRegions : ISynchronizer<SaveExOzRegionCommandResult, string>
    {
        private readonly ICommandSender _commandSender;
        private readonly IConsoleLogger _logger;

        public SyncExOzRegions(ICommandSender commandSender, IConsoleLogger logger)
        {
            _commandSender = commandSender;
            _logger = logger;
        }

        public async Task<SaveExOzRegionCommandResult> Synchronize(string obj)
        {
            string line = "";
            _logger.StartMsg("Regions");
            var postResponse = HttpWebRequestHelper.ExOz_WebRequestGet("");
            ExOzRegionList exOzRegionResponse = Mapper<ExOzRegionList>.MapFromJson(postResponse);
            var regionList = exOzRegionResponse.regions;

            //Get Region details and prepare list of Operators
            List<ExOzRegionResponse> regionDetailsList = new List<ExOzRegionResponse>();
            OperatorList operatorList = new OperatorList()
            {
                operators = new List<ExOzOperatorResponse>()
            };

            int i = 0;
            foreach (var region in exOzRegionResponse.regions)
            {
                try
                {
                    string regionDetails = HttpWebRequestHelper.ExOz_WebRequestGet(region.UrlSegment);
                    ExOzRegionResponse objRegion = Mapper<ExOzRegionResponse>.MapFromJson(regionDetails);

                    foreach (var item in objRegion.Operators)
                    {
                        item.RegionId = objRegion.Id;
                        item.RegionUrlSegment = objRegion.UrlSegment;
                        operatorList.operators.Add(item);
                    }
                    regionDetailsList.Add(objRegion);
                }
                catch (Exception e)
                {
                    _logger.Log($"Exceptions: {e.Message}");
                }
                i++;
                line = _logger.Update(i, exOzRegionResponse.regions.Count, line);
            }

            //Code to insert Regions
            try
            {
                SaveExOzRegionCommandResult retRegions = await _commandSender.Send<SaveExOzRegionCommand, SaveExOzRegionCommandResult>(new SaveExOzRegionCommand
                {
                    RegionList = regionDetailsList
                });
                _logger.FinishMsg(retRegions.RegionList.Count, "Regions");
                retRegions.OperatorList = operatorList;
                return retRegions;
            }
            catch (Exception e)
            {
                _logger.Log($"Exception: {e.Message}");
                throw;
            }
        }
    }
}
