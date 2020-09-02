using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IMasterVenueLayoutRepository : IOrmRepository<MasterVenueLayout, MasterVenueLayout>
    {
        MasterVenueLayout Get(int id);

        IEnumerable<MasterVenueLayout> GetAllByVenueId(int venueId);

        MasterVenueLayout GetByAltId(Guid AltId);

        MasterVenueLayout GetByName(string name);
    }

    public class MasterVenueLayoutRepository : BaseOrmRepository<MasterVenueLayout>, IMasterVenueLayoutRepository
    {
        public MasterVenueLayoutRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MasterVenueLayout Get(int id)
        {
            return Get(new MasterVenueLayout { Id = id });
        }

        public IEnumerable<MasterVenueLayout> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteMasterVenueLayout(MasterVenueLayout masterVenueLayout)
        {
            Delete(masterVenueLayout);
        }

        public MasterVenueLayout SaveMasterVenueLayout(MasterVenueLayout masterVenueLayout)
        {
            return Save(masterVenueLayout);
        }

        public IEnumerable<MasterVenueLayout> GetAllByVenueId(int venueId)
        {
            return GetAll(s => s.Where($"{nameof(MasterVenueLayout.VenueId):C}=@Id and Isenabled=1")
                  .WithParameters(new { Id = venueId }));
        }

        public MasterVenueLayout GetByAltId(Guid AltId)
        {
            return GetAll(statement => statement.Where($"{nameof(MasterVenueLayout.AltId):C} = @altId")
                         .WithParameters(new { altId = AltId })).FirstOrDefault();
        }

        public MasterVenueLayout GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(MasterVenueLayout.LayoutName):C} = @LayoutName")
                .WithParameters(new { LayoutName = name })
            ).FirstOrDefault();
        }
    }
}