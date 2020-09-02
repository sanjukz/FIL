using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IASIPaymentResponseDetailTicketMappingRepository : IOrmRepository<ASIPaymentResponseDetailTicketMapping, ASIPaymentResponseDetailTicketMapping>
    {
        ASIPaymentResponseDetailTicketMapping Get(long id);

        IEnumerable<ASIPaymentResponseDetailTicketMapping> GetByASIPaymentDetailIds(IEnumerable<long> ASIPaymentDetailIds);

        ASIPaymentResponseDetailTicketMapping GetByVisitorId(long visitorId);

        IEnumerable<ASIPaymentResponseDetailTicketMapping> GetByVisitorIds(IEnumerable<long> visitorIds);

        IEnumerable<ASIPaymentResponseDetailTicketMapping> GetByIds(IEnumerable<long> Ids);
    }

    public class ASIPaymentResponseDetailTicketMappingRepository : BaseLongOrmRepository<ASIPaymentResponseDetailTicketMapping>, IASIPaymentResponseDetailTicketMappingRepository
    {
        public ASIPaymentResponseDetailTicketMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASIPaymentResponseDetailTicketMapping Get(long id)
        {
            return Get(new ASIPaymentResponseDetailTicketMapping { Id = id });
        }

        public IEnumerable<ASIPaymentResponseDetailTicketMapping> GetByASIPaymentDetailIds(IEnumerable<long> ASIPaymentDetailIds)
        {
            return GetAll(statement => statement
                                .Where($"{nameof(ASIPaymentResponseDetailTicketMapping.ASIPaymentResponseDetailId):C} IN @Ids")
                                .WithParameters(new { Ids = ASIPaymentDetailIds }));
        }

        public ASIPaymentResponseDetailTicketMapping GetByVisitorId(long visitorId)
        {
            return GetAll(statement => statement
                                .Where($"{nameof(ASIPaymentResponseDetailTicketMapping.VisitorId):C} = @Id")
                                .WithParameters(new { Id = visitorId })).FirstOrDefault();
        }

        public IEnumerable<ASIPaymentResponseDetailTicketMapping> GetByVisitorIds(IEnumerable<long> visitorIds)
        {
            return GetAll(statement => statement
                                .Where($"{nameof(ASIPaymentResponseDetailTicketMapping.VisitorId):C} IN @Id")
                                .WithParameters(new { Id = visitorIds }));
        }

        public IEnumerable<ASIPaymentResponseDetailTicketMapping> GetByIds(IEnumerable<long> Ids)
        {
            return GetAll(statement => statement
                                .Where($"{nameof(ASIPaymentResponseDetailTicketMapping.Id):C} IN @Id")
                                .WithParameters(new { Id = Ids }));
        }
    }
}