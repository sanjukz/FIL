using Dapper.FastCrud;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IAlgoliaExportRepositoryRepository : IOrmRepository<AlgoliaExport, AlgoliaExport>
    {
        AlgoliaExport Get(long id);

        AlgoliaExport GetByObjectId(string objectId);

        void DisableAll();

        IEnumerable<AlgoliaExport> GetByAllDiabledObjects();
    }

    public class AlgoliaExportRepositoryRepository : BaseLongOrmRepository<AlgoliaExport>, IAlgoliaExportRepositoryRepository
    {
        public AlgoliaExportRepositoryRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public AlgoliaExport Get(long id)
        {
            return Get(new AlgoliaExport { Id = id });
        }

        public AlgoliaExport GetByObjectId(string objectId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(AlgoliaExport.ObjectId):C} = @ObjectId")
                    .WithParameters(new { ObjectId = objectId })).FirstOrDefault();
        }

        public void DisableAll()
        {
            var partialUpdateMapping = OrmConfiguration
      .GetDefaultEntityMapping<AlgoliaExport>()
      .Clone() // clone it if you don't want to modify the default
      .UpdatePropertiesExcluding(prop => prop.IsExcludedFromUpdates = true, nameof(AlgoliaExport.IsIndexed));

            GetCurrentConnection().BulkUpdate(
                new AlgoliaExport
                {
                    IsIndexed = false,
                },
                statement => statement.WithEntityMappingOverride(partialUpdateMapping)
                .Where($"{nameof(AlgoliaExport.IsEnabled):C}=1")
                .AttachToTransaction(GetCurrentTransaction()));
        }

        public IEnumerable<AlgoliaExport> GetByAllDiabledObjects()
        {
            return GetAll(statement => statement
                .Where($"{nameof(AlgoliaExport.IsEnabled):C}=1 And IsIndexed=0"));
        }
    }
}