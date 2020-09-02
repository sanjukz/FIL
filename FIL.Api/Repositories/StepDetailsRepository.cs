using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IStepDetailsRepository : IOrmRepository<StepDetail, StepDetail>
    {
        StepDetail Get(int id);

        IEnumerable<StepDetail> GetByIds(List<int> Ids);
    }

    public class StepDetailsRepository : BaseOrmRepository<StepDetail>, IStepDetailsRepository
    {
        public StepDetailsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public StepDetail Get(int id)
        {
            return Get(new StepDetail { Id = id });
        }

        public IEnumerable<StepDetail> GetByIds(List<int> Ids)
        {
            return GetAll(s => s.Where($"{nameof(StepDetail.Id):C} IN @Id")
               .WithParameters(new { Id = Ids })
           );
        }
    }
}