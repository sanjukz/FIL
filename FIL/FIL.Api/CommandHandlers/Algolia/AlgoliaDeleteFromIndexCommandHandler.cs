using FIL.Api.Providers.Algolia;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Algolia;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Algolia
{
    public class AlgoliaDeleteFromIndexCommandHandler : BaseCommandHandler<AlgoliaDeleteFromIndexCommand>
    {
        private readonly IAlgoliaExportRepositoryRepository _algoliaExportRepositoryRepository;
        private readonly IAlgoliaClientProvider _algoliaClientProvider;
        private readonly ILogger _logger;

        public AlgoliaDeleteFromIndexCommandHandler(IAlgoliaExportRepositoryRepository algoliaExportRepositoryRepository, IAlgoliaClientProvider algoliaClientProvider, ILogger logger, IMediator mediator)
            : base(mediator)
        {
            _algoliaExportRepositoryRepository = algoliaExportRepositoryRepository;
            _algoliaClientProvider = algoliaClientProvider;
            _logger = logger;
        }

        protected override Task Handle(AlgoliaDeleteFromIndexCommand command)
        {
            try
            {
                var algoliaObjects = _algoliaExportRepositoryRepository.GetByAllDiabledObjects().ToList();
                List<string> objectIds = new List<string>();
                foreach (var currentObject in algoliaObjects)
                {
                    var currentObjectModel = algoliaObjects.Where(s => s.ObjectId == currentObject.ObjectId).FirstOrDefault();
                    objectIds.Add(currentObject.ObjectId);
                    currentObjectModel.IsEnabled = false;
                    _algoliaExportRepositoryRepository.Save(currentObjectModel);
                }
                if (objectIds.Count > 0)
                {
                    _algoliaClientProvider.DeleteObjects(objectIds);
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to Delete Objects to Algolia index", e));
            }
            return Task.FromResult(0);
        }
    }
}