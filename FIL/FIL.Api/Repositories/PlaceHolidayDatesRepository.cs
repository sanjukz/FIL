using Dapper.FastCrud;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPlaceHolidayDatesRepository : IOrmRepository<PlaceHolidayDate, PlaceHolidayDate>
    {
        PlaceHolidayDate Get(long id);

        IEnumerable<PlaceHolidayDate> GetAllByEventId(long ids);

        PlaceHolidayDate GetByEventandDate(long eventId, DateTime blockDate);

        IEnumerable<PlaceHolidayDate> GetAllByEventIds(List<long> ids);

        void DisableAllDatesByEventId(long eventId);
    }

    public class PlaceHolidayDatesRepository : BaseLongOrmRepository<PlaceHolidayDate>, IPlaceHolidayDatesRepository
    {
        public PlaceHolidayDatesRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public PlaceHolidayDate Get(long id)
        {
            return Get(new PlaceHolidayDate { Id = id });
        }

        public IEnumerable<PlaceHolidayDate> GetAllByEventId(long ids)
        {
            var placeWeekOffList = GetAll(statement => statement
                .Where($"{nameof(PlaceHolidayDate.EventId):C} = @Ids")
                .WithParameters(new { Ids = ids }));
            return placeWeekOffList;
        }

        public IEnumerable<PlaceHolidayDate> GetAllByEventIds(List<long> ids)
        {
            var placeWeekOffList = GetAll(statement => statement
                .Where($"{nameof(PlaceHolidayDate.EventId):C} IN @Ids")
                .WithParameters(new { Ids = ids }));
            return placeWeekOffList;
        }

        public PlaceHolidayDate GetByEventandDate(long eventId, DateTime blockDate)
        {
            return GetAll(statement => statement
            .Where($"{nameof(PlaceHolidayDate.EventId):C} = @EventId AND {nameof(PlaceHolidayDate.LeaveDateTime):C} = @LeaveDateTime")
            .WithParameters(new { EventId = eventId, LeaveDateTime = blockDate })).FirstOrDefault();
        }

        public void DisableAllDatesByEventId(long eventId)
        {
            var partialUpdateMapping = OrmConfiguration
        .GetDefaultEntityMapping<PlaceHolidayDate>()
        .Clone() // clone it if you don't want to modify the default
        .UpdatePropertiesExcluding(prop => prop.IsExcludedFromUpdates = true, nameof(PlaceHolidayDate.IsEnabled));

            GetCurrentConnection().BulkUpdate(
                new PlaceHolidayDate
                {
                    IsEnabled = false,    // You can add as many as fields you want to update
                }, statement => statement.WithEntityMappingOverride(partialUpdateMapping).Where($"{nameof(PlaceHolidayDate.EventId):C}=@EventId").WithParameters(new { EventId = eventId }).AttachToTransaction(GetCurrentTransaction()));
        }
    }
}