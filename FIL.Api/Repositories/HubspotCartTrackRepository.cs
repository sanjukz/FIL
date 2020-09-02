using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IHubspotCartTrackRepository : IOrmRepository<HubspotCartTrack, HubspotCartTrack>
    {
        HubspotCartTrack Get(int id);

        void DeleteHubspotCartTrack(HubspotCartTrack hubSportCartTrack);

        HubspotCartTrack SaveHubspotCartTrack(HubspotCartTrack hubSportCartTrack);

        HubspotCartTrack GetByVId(long vId);

        IEnumerable<HubspotCartTrack> GetAbandonCart();
    }

    public class HubspotCartTrackRepository : BaseOrmRepository<HubspotCartTrack>, IHubspotCartTrackRepository
    {
        public HubspotCartTrackRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public HubspotCartTrack Get(int id)
        {
            return Get(new HubspotCartTrack { Id = id });
        }

        public IEnumerable<HubspotCartTrack> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteHubspotCartTrack(HubspotCartTrack hubSportCartTrack)
        {
            Delete(hubSportCartTrack);
        }

        public HubspotCartTrack SaveHubspotCartTrack(HubspotCartTrack hubSportCartTrack)
        {
            return Save(hubSportCartTrack);
        }

        public HubspotCartTrack GetByVId(long vId)
        {
            return GetAll(statement => statement
                          .Where($"{nameof(HubspotCartTrack.HubspotVid):C} = @vId")
                          .WithParameters(new { vId = vId })).FirstOrDefault();
        }

        public IEnumerable<HubspotCartTrack> GetAbandonCart()
        {
            return GetAll(s => s.Where($"{nameof(HubspotCartTrack.CreatedUtc):C} >= @FromDate AND {nameof(HubspotCartTrack.CreatedUtc):C} <= @ToDate")
            .WithParameters(new { FromDate = DateTime.UtcNow.AddMinutes(-6000), ToDate = DateTime.UtcNow.AddMinutes(-20) }));
        }
    }
}