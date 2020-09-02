using FIL.Api.Integrations.HttpHelpers;
using FIL.Api.Integrations.ValueRetail;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.ValueRetail;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations.ValueRetail;
using FIL.Contracts.Models.ValueRetail;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;

//using Stripe;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ValueRetail
{
    public class VillageCommandHandler : BaseCommandHandler<VillageCommand>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IMediator _mediator;
        private readonly IValueRetailVillageRepository _valueRetailVillageRepository;
        private readonly IValueRetailAPI _valueRetailAPI;

        public VillageCommandHandler(
            ILogger logger,
            ISettings settings,
            IMediator mediator,
            IValueRetailVillageRepository valueRetailVillageRepository,
            IValueRetailAPI valueRetailAPI
           )
           : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _mediator = mediator;
            _valueRetailVillageRepository = valueRetailVillageRepository;
            _valueRetailAPI = valueRetailAPI;
        }

        protected override async Task Handle(VillageCommand command)
        {
            await FetchAndSaveVillages();
        }

        public async Task FetchAndSaveVillages()
        {
            var responseString = await _valueRetailAPI.GetValueRetailAPIData(new ValueRetailCommanRequestModel(), "all", "Villages");

            VillageResponse responseData = new VillageResponse();
            responseData = Mapper<VillageResponse>.MapJsonStringToObject(responseString.Result);

            foreach (var village in responseData.Villages)
            {
                try
                {
                    var VRVillage = _valueRetailVillageRepository.GetByVillageCode(village.VillageCode);
                    if (VRVillage == null)
                    {
                        VRVillage = _valueRetailVillageRepository.Save(new ValueRetailVillage
                        {
                            AltId = Guid.NewGuid(),
                            VillageName = village.VillageName,
                            VillageCode = village.VillageCode,
                            CultureCode = village.CultureCode,
                            CurrencyCode = village.CurrencyCode,
                            ModifiedBy = Guid.NewGuid(),
                            IsEnabled = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogCategory.Error, new Exception("Failed to save Value Retail Villages in Db", ex));
                }
            }
        }
    }
}