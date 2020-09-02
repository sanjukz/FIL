using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IPlaceDocumentTypeMappingRepository : IOrmRepository<PlaceDocumentTypeMapping, PlaceDocumentTypeMapping>
    {
        PlaceDocumentTypeMapping Get(long id);

        IEnumerable<PlaceDocumentTypeMapping> GetByEventId(long eventId);
    }

    public class PlaceDocumentTypeMappingRepository : BaseLongOrmRepository<PlaceDocumentTypeMapping>, IPlaceDocumentTypeMappingRepository
    {
        public PlaceDocumentTypeMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public PlaceDocumentTypeMapping Get(long id)
        {
            return Get(new PlaceDocumentTypeMapping { Id = id });
        }

        public IEnumerable<PlaceDocumentTypeMapping> GetByEventId(long eventId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(PlaceDocumentTypeMapping.EventId):C}=@Ids")
                .WithParameters(new { Ids = eventId }));
        }
    }
}