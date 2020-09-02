using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IFILSponsorDetailRepository : IOrmRepository<FILSponsorDetail, FILSponsorDetail>
    {
        FILSponsorDetail Get(long id);

        IEnumerable<FILSponsorDetail> GetAllByEventId(long eventId);

        FILSponsorDetail GetByAltId(Guid altId);
    }

    public class FILSponsorDetailRepository : BaseLongOrmRepository<FILSponsorDetail>, IFILSponsorDetailRepository
    {
        public FILSponsorDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public FILSponsorDetail Get(long id)
        {
            return Get(new FILSponsorDetail { Id = id });
        }

        public IEnumerable<FILSponsorDetail> GetAllByEventId(long eventId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(FILSponsorDetail.EventId):C} = @EventIdParam")
                    .WithParameters(new { EventIdParam = eventId }));
        }

        public FILSponsorDetail GetByAltId(Guid altId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(FILSponsorDetail.AltId):C} = @EventAltId")
                    .WithParameters(new { EventAltId = altId })).FirstOrDefault();
        }
    }
}