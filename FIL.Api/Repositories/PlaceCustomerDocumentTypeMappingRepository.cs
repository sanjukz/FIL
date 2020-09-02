using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPlaceCustomerDocumentTypeMappingRepository : IOrmRepository<PlaceCustomerDocumentTypeMapping, PlaceCustomerDocumentTypeMapping>
    {
        PlaceCustomerDocumentTypeMapping Get(long id);

        IEnumerable<PlaceCustomerDocumentTypeMapping> GetAllByEventId(long eventId);

        PlaceCustomerDocumentTypeMapping GetByEventIdAndPlaceCustomerDocumentId(long eventId, long documentId);
    }

    public class PlaceCustomerDocumentTypeMappingRepository : BaseLongOrmRepository<PlaceCustomerDocumentTypeMapping>, IPlaceCustomerDocumentTypeMappingRepository
    {
        public PlaceCustomerDocumentTypeMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public PlaceCustomerDocumentTypeMapping Get(long id)
        {
            return Get(new PlaceCustomerDocumentTypeMapping { Id = id });
        }

        public IEnumerable<PlaceCustomerDocumentTypeMapping> GetAllByEventId(long eventId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(PlaceCustomerDocumentTypeMapping.EventId):C}=@Ids")
                .WithParameters(new { Ids = eventId }));
        }

        public PlaceCustomerDocumentTypeMapping GetByEventIdAndPlaceCustomerDocumentId(long eventId, long documentId)
        {
            return GetAll(s => s.Where($"{nameof(PlaceCustomerDocumentTypeMapping.EventId):C} = @EventId AND  {nameof(PlaceCustomerDocumentTypeMapping.CustomerDocumentType):C} = @DocumentId")
                .WithParameters(new { EventId = eventId, DocumentId = documentId })
            ).FirstOrDefault();
        }
    }
}