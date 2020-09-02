using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IReferralRepository : IOrmRepository<Referral, Referral>
    {
        Referral Get(long id);

        IEnumerable<Referral> GetAllByEventId(long EventId);

        Referral GetByAltId(Guid AltId);

        Referral GetBySlug(string slug);
    }

    public class ReferralRepository : BaseLongOrmRepository<Referral>, IReferralRepository
    {
        public ReferralRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Referral Get(long id)
        {
            return Get(new Referral { Id = id });
        }

        public IEnumerable<Referral> GetAllByEventId(long EventId)
        {
            return GetAll(s => s.Where($"{nameof(Referral.EventId):C} = @Id")
               .WithParameters(new { Id = EventId }));
        }

        public Referral GetByAltId(Guid AltId)
        {
            return GetAll(s => s.Where($"{nameof(Referral.AltId):C} = @Id")
               .WithParameters(new { Id = AltId })).FirstOrDefault();
        }

        public Referral GetBySlug(string slug)
        {
            return GetAll(s => s.Where($"{nameof(Referral.Slug):C} = @Id")
               .WithParameters(new { Id = slug })).FirstOrDefault();
        }
    }
}