using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Algolia;
using FIL.Contracts.Queries.Algolia;
using FIL.Foundation.Senders;
using FIL.Web.Feel.ViewModels.Algolia;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Feel.Controllers
{
    public class AlgoliaController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;


        public AlgoliaController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpGet]
        [Route("api/algolia/sync-places/{skipIndex}/{takeIndex}/{isCities}")]
        public async Task<PlacesSyncReponseModel> SyncPlaces(int skipIndex, int takeIndex, bool isCities)
        {
            try
            {
                var queryResult = await _querySender.Send(new GetAllPlacesQuery
                {
                    IsFeel = true,
                    SkipIndex = skipIndex,
                    TakeIndex = takeIndex,
                    IsCities = isCities
                });
                if (!isCities)
                {
                    AlgoliaPlaceSyncCommandResult result = await _commandSender.Send<AlgoliaPlaceSyncCommand, AlgoliaPlaceSyncCommandResult>(new AlgoliaPlaceSyncCommand { AllPlaces = queryResult.AllPlaces, ModifiedBy = new Guid("D21A85EE-351C-4349-9953-FE1492740976") }, new TimeSpan(2, 0, 0));
                }
                else
                {
                    AlgoliaCitiesSyncCommandResult result = await _commandSender.Send<AlgoliaCitiesSyncCommand, AlgoliaCitiesSyncCommandResult>(new AlgoliaCitiesSyncCommand { AllCities = queryResult.GetAllCities, ModifiedBy = new Guid("D21A85EE-351C-4349-9953-FE1492740976") }, new TimeSpan(2, 0, 0));
                }
                return new PlacesSyncReponseModel
                {
                    IsSuccess = true,
                    AllPlaces = queryResult.AllPlaces
                };
            }
            catch (Exception ex)
            {
                return new PlacesSyncReponseModel
                {
                    IsSuccess = false
                };
            }
        }
        [HttpGet]
        [Route("api/algolia/disable-index")]
        public async Task<PlacesSyncReponseModel> InitialDisableIndex()
        {
            try
            {
                await _commandSender.Send(new AlgoliaDisableIndexCommand { });

                return new PlacesSyncReponseModel
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new PlacesSyncReponseModel
                {
                    IsSuccess = false
                };
            }
        }

        [HttpGet]
        [Route("api/algolia/delete-from-index")]
        public async Task<PlacesSyncReponseModel> DeleteFromAlgoliaIndex()
        {
            try
            {
                await _commandSender.Send(new AlgoliaDeleteFromIndexCommand { });

                return new PlacesSyncReponseModel
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new PlacesSyncReponseModel
                {
                    IsSuccess = false
                };
            }
        }
    }
}
