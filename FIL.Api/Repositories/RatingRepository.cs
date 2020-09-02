using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IRatingRepository : IOrmRepository<Rating, Rating>
    {
        Rating Get(int id);

        IEnumerable<Rating> GetByEventId(long eventId);

        Rating GetByAltId(Guid altId);

        IEnumerable<Rating> GetByEventIds(IEnumerable<long> eventIds);
    }

    public class RatingRepository : BaseOrmRepository<Rating>, IRatingRepository
    {
        public RatingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Rating Get(int id)
        {
            return Get(new Rating { Id = id });
        }

        public IEnumerable<Rating> GetByEventId(long eventId)
        {
            var ratingList = GetAll(statement => statement
                .Where($"{nameof(Rating.EventId):C} = @Id")
                .WithParameters(new { Id = eventId }));
            return ratingList;
        }

        public Rating GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(Rating.AltId):C} = @AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public IEnumerable<Rating> GetByEventIds(IEnumerable<long> eventIds)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(Rating.EventId):C} IN @Ids")
                    .WithParameters(new { Ids = eventIds }));
        }

        public IEnumerable<Rating> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteRating(Rating discount)
        {
            Delete(discount);
        }

        public Rating SaveRating(Rating discount)
        {
            return Save(discount);
        }
    }
}