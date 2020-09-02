using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IMatchLayoutSectionRepository : IOrmRepository<MatchLayoutSection, MatchLayoutSection>
    {
        MatchLayoutSection Get(int id);

        List<SectionDetailsByMatchLayout> GetByEventDetailId(int eventDetailId);
    }

    public class MatchLayoutSectionRepository : BaseOrmRepository<MatchLayoutSection>, IMatchLayoutSectionRepository
    {
        public MatchLayoutSectionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MatchLayoutSection Get(int id)
        {
            return Get(new MatchLayoutSection { Id = id });
        }

        public IEnumerable<MatchLayoutSection> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteMatchLayoutSection(MatchLayoutSection matchLayoutSection)
        {
            Delete(matchLayoutSection);
        }

        public MatchLayoutSection SaveMatchLayoutSection(MatchLayoutSection matchLayoutSection)
        {
            return Save(matchLayoutSection);
        }

        public List<SectionDetailsByMatchLayout> GetByEventDetailId(int eventDetailId)
        {
            //List<SectionDetailsByVenueLayout> sectionDetailsByVenueLayout = new List<SectionDetailsByVenueLayout>();
            var data = GetCurrentConnection().QueryMultiple("spMatchLayoutSectionsByMatchLayoutId", new { EventDetailId = eventDetailId }, commandType: System.Data.CommandType.StoredProcedure);
            return data.Read<SectionDetailsByMatchLayout>().ToList();
        }
    }
}