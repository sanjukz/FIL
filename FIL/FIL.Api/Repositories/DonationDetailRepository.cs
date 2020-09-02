using System.Collections.Generic;
using FIL.Api.Core.Utilities;
using FIL.Api.Core.Repositories;
using FIL.Contracts.DataModels;
using System;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IDonationDetailRepository : IOrmRepository<DonationDetail, DonationDetail>
    {
        DonationDetail Get(long id);
        DonationDetail GetByEventId(long Id);
    }

    public class DonationDetailRepository : BaseLongOrmRepository<DonationDetail>, IDonationDetailRepository
    {
        public DonationDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public DonationDetail Get(long id)
        {
            return Get(new DonationDetail { Id = id });
        }

        public DonationDetail GetByEventId(long Id)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(DonationDetail.EventId):C} = @EventId")
                    .WithParameters(new { EventId = Id })).FirstOrDefault();
        }
    }
}