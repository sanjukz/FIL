using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IValueRetailReturnStopRepository : IOrmRepository<ValueRetailReturnStop, ValueRetailReturnStop>
    {
        ValueRetailReturnStop Get(int id);

        ValueRetailReturnStop GetByStopId(int stopId);

        IEnumerable<ValueRetailReturnStop> GetByValueRetailRouteId(int valueRetailRouteId);

        IEnumerable<ValueRetailReturnStop> GetByValueRetailRouteIdAndRouteId(int valueRetailRouteId, int routeId);
    }

    public class ValueRetailReturnStopRepository : BaseOrmRepository<ValueRetailReturnStop>, IValueRetailReturnStopRepository
    {
        public ValueRetailReturnStopRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ValueRetailReturnStop Get(int id)
        {
            return Get(new ValueRetailReturnStop { Id = id });
        }

        public ValueRetailReturnStop GetByStopId(int stopId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailReturnStop.StopId):C} = @stopIdParam").WithParameters(new { stopIdParam = stopId })).FirstOrDefault();
        }

        public IEnumerable<ValueRetailReturnStop> GetByValueRetailRouteId(int valueRetailRouteId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailReturnStop.ValueRetailRouteId):C} = @ValueRetailRouteIdParam").WithParameters(new { ValueRetailRouteIdParam = valueRetailRouteId }));
        }

        public IEnumerable<ValueRetailReturnStop> GetByValueRetailRouteIdAndRouteId(int valueRetailRouteId, int routeId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailReturnStop.ValueRetailRouteId):C} = @ValueRetailRouteIdParam AND {nameof(ValueRetailReturnStop.RouteId):C} = @RouteIdParam")
            .WithParameters(new { ValueRetailRouteIdParam = valueRetailRouteId, RouteIdParam = routeId }));
        }

        public IEnumerable<ValueRetailReturnStop> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteValueRetailReturnStop(ValueRetailReturnStop valueRetailReturnStop)
        {
            Delete(valueRetailReturnStop);
        }

        public ValueRetailReturnStop SaveValueRetailPrimaryRoute(ValueRetailReturnStop valueRetailReturnStop)
        {
            return Save(valueRetailReturnStop);
        }
    }
}