using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IMasterVenueLayoutSectionSeatRepository : IOrmRepository<MasterVenueLayoutSectionSeat, MasterVenueLayoutSectionSeat>
    {
        MasterVenueLayoutSectionSeat Get(long id);

        IEnumerable<MasterVenueLayoutSectionSeat> GetByMasterVenueLayoutSectionId(int masterVenueLayoutSectionId);

        int GetSeatCount(int id);
    }

    public class MasterVenueLayoutSectionSeatRepository : BaseLongOrmRepository<MasterVenueLayoutSectionSeat>, IMasterVenueLayoutSectionSeatRepository
    {
        public MasterVenueLayoutSectionSeatRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MasterVenueLayoutSectionSeat Get(long id)
        {
            return Get(new MasterVenueLayoutSectionSeat { Id = id });
        }

        public IEnumerable<MasterVenueLayoutSectionSeat> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteMasterVenueLayoutSectionSeat(MasterVenueLayoutSectionSeat masterVenueLayoutSectionSeat)
        {
            Delete(masterVenueLayoutSectionSeat);
        }

        public MasterVenueLayoutSectionSeat SaveMasterVenueLayoutSectionSeat(MasterVenueLayoutSectionSeat masterVenueLayoutSectionSeat)
        {
            return Save(masterVenueLayoutSectionSeat);
        }

        public IEnumerable<MasterVenueLayoutSectionSeat> GetByMasterVenueLayoutSectionId(int masterVenueLayoutSectionId)
        {
            return GetAll(s => s.Where($"{nameof(MasterVenueLayoutSectionSeat.MasterVenueLayoutSectionId):C}=@MasterVenueLayoutSectionId")
                  .WithParameters(new { MasterVenueLayoutSectionId = masterVenueLayoutSectionId }));
        }

        public int GetSeatCount(int id)
        {
            var count = GetCurrentConnection().Query<int>("Select count(*) from MasterVenueLayoutSectionSeats where MasterVenueLayoutSectionId=@Id", new
            {
                Id = id
            }).FirstOrDefault();
            // return Convert.ToInt32(count);
            return count;
        }
    }
}