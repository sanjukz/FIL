using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IGuestUserAdditionalDetailRepository : IOrmRepository<GuestUserAdditionalDetail, GuestUserAdditionalDetail>
    {
        GuestUserAdditionalDetail Get(long id);

        IEnumerable<GuestUserAdditionalDetail> GetByUserId(long userId);

        GuestUserAdditionalDetail GetByAltId(Guid altId);
    }

    public class GuestUserAdditionalDetailRepository : BaseLongOrmRepository<GuestUserAdditionalDetail>, IGuestUserAdditionalDetailRepository
    {
        public GuestUserAdditionalDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public GuestUserAdditionalDetail Get(long id)
        {
            return Get(new GuestUserAdditionalDetail { Id = id });
        }

        public IEnumerable<GuestUserAdditionalDetail> GetByUserId(long userId)
        {
            return GetAll(s => s.Where($"{nameof(GuestUserAdditionalDetail.UserId):C} = @UserId")
            .WithParameters(new { UserId = userId })
            );
        }

        public GuestUserAdditionalDetail GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(GuestUserAdditionalDetail.AltId):C} = @AltId")
            .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }
    }
}