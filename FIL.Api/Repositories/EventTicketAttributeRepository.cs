using Dapper.FastCrud;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventTicketAttributeRepository : IOrmRepository<EventTicketAttribute, EventTicketAttribute>
    {
        EventTicketAttribute Get(long id);

        EventTicketAttribute GetByEventTicketDetailId(long eventTicketDetailId);

        IEnumerable<EventTicketAttribute> GetByEventTicketDetailId(IEnumerable<long> eventTicketDetailIds);

        IEnumerable<EventTicketAttribute> GetByEventTicketDetailIds(IEnumerable<long> ids);

        IEnumerable<EventTicketAttribute> GetByEventTicketAttributeIds(IEnumerable<long> ids);

        EventTicketAttribute GetMaxPriceByEventDetailId(IEnumerable<long> eventDetailIds);

        EventTicketAttribute GetMinPriceByEventDetailId(IEnumerable<long> eventDetailIds);

        IEnumerable<EventTicketAttribute> GetByIds(IEnumerable<long> ids);

        EventTicketAttribute GetByEventTicketDetailsId(long eventTicketDetailId);

        IEnumerable<EventTicketAttribute> GetByEventTicketAttributeId(long id);

        IEnumerable<EventTicketAttribute> GetSeasonByEventTicketDetailId(IEnumerable<long> eventTicketDetailIds);

        EventTicketAttribute GetByEventTicketDetailIdFeelAdmin(long eventTicketDetailId);

        EventTicketAttribute GetMaxPriceByEventTicketDetailId(IEnumerable<long> eventTicketDetailIds);

        EventTicketAttribute GetMinPriceByEventTicketDetailId(IEnumerable<long> eventTicketDetailIds);
    }

    public class EventTicketAttributeRepository : BaseLongOrmRepository<EventTicketAttribute>, IEventTicketAttributeRepository
    {
        public EventTicketAttributeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventTicketAttribute Get(long id)
        {
            return Get(new EventTicketAttribute { Id = id });
        }

        public IEnumerable<EventTicketAttribute> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventTicketAttribute(EventTicketAttribute eventTicketAttribute)
        {
            Delete(eventTicketAttribute);
        }

        public EventTicketAttribute SaveEventTicketAttribute(EventTicketAttribute eventTicketAttribute)
        {
            return Save(eventTicketAttribute);
        }

        public EventTicketAttribute GetMaxPriceByEventDetailId(IEnumerable<long> eventDetailIds)
        {
            return GetAll(statement => statement
                .Where($@"{nameof(EventTicketAttribute.Id):C} IN (
                    SELECT Id
                      FROM {Sql.Table<EventTicketAttribute>()}
                        WHERE {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.EventTicketDetailId))} IN (
                            SELECT {Sql.Column<EventTicketDetail>(nameof(EventTicketDetail.Id))}
                                FROM {Sql.Table<EventTicketDetail>()} WHERE
                                {Sql.Column<EventTicketDetail>(nameof(EventTicketDetail.EventDetailId))}
                                IN @EventDetailIds
                        )
                        AND {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.IsEnabled))} = 1
                )").OrderBy($"{nameof(EventTicketAttribute.Price):C} DESC").Top(1)
                .WithParameters(new { EventDetailIds = eventDetailIds })).FirstOrDefault();
        }

        public EventTicketAttribute GetMinPriceByEventDetailId(IEnumerable<long> eventDetailIds)
        {
            EventTicketAttribute eventTickets = new EventTicketAttribute();
            eventTickets = GetAll(statement => statement
                .Where($@"{nameof(EventTicketAttribute.Id):C} IN (
                    SELECT Id
                      FROM {Sql.Table<EventTicketAttribute>()}
                        WHERE {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.EventTicketDetailId))} IN (
                            SELECT {Sql.Column<EventTicketDetail>(nameof(EventTicketDetail.Id))}
                                FROM {Sql.Table<EventTicketDetail>()} WHERE
                                {Sql.Column<EventTicketDetail>(nameof(EventTicketDetail.EventDetailId))}
                                IN @EventDetailIds
                        )
                        AND {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.Price))} > 0
                        AND {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.IsEnabled))} = 1
                )").OrderBy($"{nameof(EventTicketAttribute.Price):C} ASC").Top(1)
                .WithParameters(new { EventDetailIds = eventDetailIds })).FirstOrDefault();
            //eventTickets.Price = eventTickets.Price > 0 ?  eventTickets.Price : 0;
            return eventTickets != null ? eventTickets : new EventTicketAttribute { Price = 0 };
        }

        public EventTicketAttribute GetMaxPriceByEventTicketDetailId(IEnumerable<long> eventTicketDetailIds)
        {
            return GetAll(statement => statement
                .Where($@"{nameof(EventTicketAttribute.Id):C} IN (
                    SELECT Id
                      FROM {Sql.Table<EventTicketAttribute>()}
                        WHERE {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.EventTicketDetailId))} IN (
                            SELECT {Sql.Column<EventTicketDetail>(nameof(EventTicketDetail.Id))}
                                FROM {Sql.Table<EventTicketDetail>()} WHERE
                                {Sql.Column<EventTicketDetail>(nameof(EventTicketDetail.Id))}
                                IN @ETDIds
                        )
                        AND {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.IsEnabled))} = 1
                )").OrderBy($"{nameof(EventTicketAttribute.Price):C} DESC").Top(1)
                .WithParameters(new { ETDIds = eventTicketDetailIds })).FirstOrDefault();
        }

        public EventTicketAttribute GetMinPriceByEventTicketDetailId(IEnumerable<long> eventTicketDetailIds)
        {
            EventTicketAttribute eventTickets = new EventTicketAttribute();
            eventTickets = GetAll(statement => statement
                .Where($@"{nameof(EventTicketAttribute.Id):C} IN (
                    SELECT Id
                      FROM {Sql.Table<EventTicketAttribute>()}
                        WHERE {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.EventTicketDetailId))} IN (
                            SELECT {Sql.Column<EventTicketDetail>(nameof(EventTicketDetail.Id))}
                                FROM {Sql.Table<EventTicketDetail>()} WHERE
                                {Sql.Column<EventTicketDetail>(nameof(EventTicketDetail.Id))}
                                IN @ETDIds
                        )
                        AND {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.Price))} > 0
                        AND {Sql.Column<EventTicketAttribute>(nameof(EventTicketAttribute.IsEnabled))} = 1
                )").OrderBy($"{nameof(EventTicketAttribute.Price):C} ASC").Top(1)
                .WithParameters(new { ETDIds = eventTicketDetailIds })).FirstOrDefault();
            //eventTickets.Price = eventTickets.Price > 0 ?  eventTickets.Price : 0;
            return eventTickets != null ? eventTickets : new EventTicketAttribute { Price = 0 };
        }

        public IEnumerable<EventTicketAttribute> GetByEventTicketDetailId(IEnumerable<long> eventTicketDetailIds)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventTicketAttribute.EventTicketDetailId):C} IN @Id AND IsEnabled = 1")
                .WithParameters(new { Id = eventTicketDetailIds }));
        }

        public IEnumerable<EventTicketAttribute> GetByEventTicketDetailIds(IEnumerable<long> ids)
        {
            return GetAll(statement => statement
                        .Where($"{nameof(EventTicketAttribute.EventTicketDetailId):C} IN @EventTicketDetailIds AND IsEnabled=1")
                        .WithParameters(new { EventTicketDetailIds = ids }));
        }

        public IEnumerable<EventTicketAttribute> GetByEventTicketAttributeIds(IEnumerable<long> ids)
        {
            return GetAll(statement => statement
                        .Where($"{nameof(EventTicketAttribute.Id):C} IN @EventTicketAttributeIds")
                        .WithParameters(new { EventTicketAttributeIds = ids }));
        }

        public IEnumerable<EventTicketAttribute> GetByIds(IEnumerable<long> ids)
        {
            var EventTicketAttributeList = GetAll(statement => statement
                                 .Where($"{nameof(EventTicketAttribute.Id):C} IN @Ids")
                                 .WithParameters(new { Ids = ids }));
            return EventTicketAttributeList;
        }

        public EventTicketAttribute GetByEventTicketDetailsId(long eventTicketDetailId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventTicketAttribute.EventTicketDetailId):C} = @Id")
                .WithParameters(new { Id = eventTicketDetailId })).FirstOrDefault();
        }

        public EventTicketAttribute GetByEventTicketDetailId(long eventTicketDetailId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventTicketAttribute.EventTicketDetailId):C} = @Id")
                .WithParameters(new { Id = eventTicketDetailId })).FirstOrDefault();
        }

        public EventTicketAttribute GetByEventTicketDetailIdFeelAdmin(long eventTicketDetailId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventTicketAttribute.EventTicketDetailId):C} = @Id  AND IsEnabled = 1")
                .WithParameters(new { Id = eventTicketDetailId })).FirstOrDefault();
        }

        public IEnumerable<EventTicketAttribute> GetByEventTicketAttributeId(long id)
        {
            return GetAll(statement => statement
                        .Where($"{nameof(EventTicketAttribute.Id):C} IN @EventTicketAttributeIds")
                        .WithParameters(new { EventTicketAttributeIds = id }));
        }

        public IEnumerable<EventTicketAttribute> GetSeasonByEventTicketDetailId(IEnumerable<long> eventTicketDetailIds)
        {
            return GetAll(statement => statement
                                   .Where($"{nameof(EventTicketAttribute.EventTicketDetailId):C} IN @EventTicketDetailIds AND {nameof(EventTicketAttribute.SeasonPackage):C}= 1")
                                   .WithParameters(new { EventTicketDetailIds = eventTicketDetailIds }));
        }
    }
}