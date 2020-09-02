using Dapper.FastCrud;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IAlgoliaCitiesExportRepository : IOrmRepository<AlgoliaCitiesExport, AlgoliaCitiesExport>
    {
        AlgoliaCitiesExport Get(long id);

        AlgoliaCitiesExport GetByObjectId(string objectId);

        void DisableAll();

        IEnumerable<AlgoliaCitiesExport> GetByAllDiabledObjects();
    }

    public class AlgoliaCitiesExportRepositoryRepository : BaseLongOrmRepository<AlgoliaCitiesExport>, IAlgoliaCitiesExportRepository
    {
        public AlgoliaCitiesExportRepositoryRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public AlgoliaCitiesExport Get(long id)
        {
            return Get(new AlgoliaCitiesExport { Id = id });
        }

        public AlgoliaCitiesExport GetByObjectId(string objectId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(AlgoliaCitiesExport.ObjectId):C} = @ObjectId")
                    .WithParameters(new { ObjectId = objectId })).FirstOrDefault();
        }

        public void DisableAll()
        {
            var partialUpdateMapping = OrmConfiguration
      .GetDefaultEntityMapping<AlgoliaCitiesExport>()
      .Clone() // clone it if you don't want to modify the default
      .UpdatePropertiesExcluding(prop => prop.IsExcludedFromUpdates = true, nameof(AlgoliaCitiesExport.IsIndexed));

            GetCurrentConnection().BulkUpdate(
                new AlgoliaCitiesExport
                {
                    IsIndexed = false,
                },
                statement => statement.WithEntityMappingOverride(partialUpdateMapping)
                .Where($"{nameof(AlgoliaCitiesExport.IsEnabled):C}=1")
                .AttachToTransaction(GetCurrentTransaction()));
        }

        public IEnumerable<AlgoliaCitiesExport> GetByAllDiabledObjects()
        {
            return GetAll(statement => statement
                .Where($"{nameof(AlgoliaCitiesExport.IsEnabled):C}=1 And IsIndexed=0"));
        }
    }
}