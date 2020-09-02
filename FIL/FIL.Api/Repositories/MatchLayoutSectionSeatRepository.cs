using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IMatchLayoutSectionSeatRepository : IOrmRepository<MatchLayoutSectionSeat, MatchLayoutSectionSeat>
    {
        MatchLayoutSectionSeat Get(long id);

        IEnumerable<MatchLayoutSectionSeat> GetByIds(IEnumerable<long> ids);

        IEnumerable<MatchLayoutSectionSeat> GetByEventTicketDetails(long eventTicketDetailId, short totalTicket);

        IEnumerable<MatchLayoutSectionSeat> GetByEventTicketDetailId(long eventTicketDetailId);

        int GetSeatCount(int id);

        IEnumerable<MatchLayoutSectionSeat> GetByMatchLayoutSectionId(int matchLayoutSectionId);

        MatchLayoutSectionSeat GetByIdandEventTicketDetailId(long id, long eventTicketDetailId);
    }

    public class MatchLayoutSectionSeatRepository : BaseLongOrmRepository<MatchLayoutSectionSeat>, IMatchLayoutSectionSeatRepository
    {
        public MatchLayoutSectionSeatRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MatchLayoutSectionSeat Get(long id)
        {
            return Get(new MatchLayoutSectionSeat { Id = id });
        }

        public IEnumerable<MatchLayoutSectionSeat> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<MatchLayoutSectionSeat> GetByIds(IEnumerable<long> ids)
        {
            return GetAll(s => s.Where($"{nameof(MatchLayoutSectionSeat.Id):C}IN @Ids")
                  .WithParameters(new { Ids = ids })
              );
        }

        public IEnumerable<MatchLayoutSectionSeat> GetByEventTicketDetails(long eventTicketDetailId, short totalTicket)
        {
            return GetAll(s => s.Where($"{nameof(MatchLayoutSectionSeat.EventTicketDetailId):C} = @EventTicketDetailId AND SeatStatusId = 1 AND SeatTypeId In(1,6)")
                  .WithParameters(new { EventTicketDetailId = eventTicketDetailId }
                  ))
                  .OrderByDescending(s => s.RowOrder).Take(totalTicket);
        }

        public IEnumerable<MatchLayoutSectionSeat> GetByEventTicketDetailId(long eventTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(MatchLayoutSectionSeat.EventTicketDetailId):C} = @EventTicketDetailId ")
                  .WithParameters(new { EventTicketDetailId = eventTicketDetailId })
              );
        }

        public void DeleteMatchLayoutSectionSeat(MatchLayoutSectionSeat matchLayoutSectionSeat)
        {
            Delete(matchLayoutSectionSeat);
        }

        public MatchLayoutSectionSeat SaveMatchLayoutSectionSeat(MatchLayoutSectionSeat matchLayoutSectionSeat)
        {
            return Save(matchLayoutSectionSeat);
        }

        public int GetSeatCount(int id)
        {
            var count = GetCurrentConnection().Query<int>("Select count(*) from MatchLayoutSectionSeats where MatchLayoutSectionId=@Id", new
            {
                Id = id
            }).FirstOrDefault();
            return count;
        }

        public IEnumerable<MatchLayoutSectionSeat> GetByMatchLayoutSectionId(int matchLayoutSectionId)
        {
            return GetAll(s => s.Where($"{nameof(MatchLayoutSectionSeat.MatchLayoutSectionId):C}=@MatchLayoutSectionId")
                  .WithParameters(new { MatchLayoutSectionId = matchLayoutSectionId }));
        }

        public MatchLayoutSectionSeat GetByIdandEventTicketDetailId(long id, long eventTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(MatchLayoutSectionSeat.Id):C} = @Id and {nameof(MatchLayoutSectionSeat.EventTicketDetailId):C} = @EventTicketDetailId")
                    .WithParameters(new { Id = id, EventTicketDetailId = eventTicketDetailId })
                ).FirstOrDefault();
        }
    }
}