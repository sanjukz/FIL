using FIL.Api.Repositories;
using FIL.Contracts.Commands.Algolia;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Algolia
{
    public class AlgoliaDisableIndexCommandHandler : BaseCommandHandler<AlgoliaDisableIndexCommand>
    {
        private readonly IAlgoliaExportRepositoryRepository _algoliaExportRepositoryRepository;

        public AlgoliaDisableIndexCommandHandler(IAlgoliaExportRepositoryRepository algoliaExportRepositoryRepository, IMediator mediator)
            : base(mediator)
        {
            _algoliaExportRepositoryRepository = algoliaExportRepositoryRepository;
        }

        protected override async Task Handle(AlgoliaDisableIndexCommand command)
        {
            _algoliaExportRepositoryRepository.DisableAll();
        }
    }
}