using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IAmenityRepository : IOrmRepository<Amenities, Amenities>
    {
        Amenities Get(int id);

        IEnumerable<Amenities> GetByEventId(long eventId);

        Amenities SaveAmenity(Amenities Amenity);
    }

    public class AmenityRepository : BaseOrmRepository<Amenities>, IAmenityRepository
    {
        public AmenityRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Amenities Get(int id)
        {
            return Get(new Amenities { Id = id });
        }

        public IEnumerable<Amenities> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventAmenity(Amenities discount)
        {
            Delete(discount);
        }

        public Amenities SaveAmenity(Amenities amenity)
        {
            return Save(amenity);
        }

        public IEnumerable<Amenities> GetByEventId(long eventId)
        {
            var eventAmenityList = GetAll(statement => statement
                .Where($"{nameof(Amenities.Id):C}= @Id")
                .WithParameters(new { Id = eventId }));
            return eventAmenityList;
        }
    }
}