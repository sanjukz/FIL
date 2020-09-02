using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IStepRepository : IOrmRepository<Step, Step>
    {
        Step Get(int id);

        IEnumerable<Step> GetByIds(List<int> Ids);
    }

    public class StepRepository : BaseOrmRepository<Step>, IStepRepository
    {
        public StepRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Step Get(int id)
        {
            return Get(new Step { Id = id });
        }

        public IEnumerable<Step> GetByIds(List<int> Ids)
        {
            return GetAll(s => s.Where($"{nameof(Step.Id):C} IN @Id")
               .WithParameters(new { Id = Ids })
           );
        }
    }
}