using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IStateDescriptionRepository : IOrmRepository<StateDescription, StateDescription>
    {
        StateDescription Get(int id);

        StateDescription GetByStateId(int stateId);
    }

    public class StateDescriptionRepository : BaseOrmRepository<StateDescription>, IStateDescriptionRepository
    {
        public StateDescriptionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public StateDescription Get(int id)
        {
            return Get(new StateDescription { Id = id });
        }

        public IEnumerable<StateDescription> GetAll()
        {
            return GetAll(null);
        }

        public StateDescription GetByStateId(int stateId)
        {
            return GetAll(s => s.Where($"{nameof(StateDescription.StateId):C} = @Id")
                  .WithParameters(new { Id = stateId })).FirstOrDefault();
        }
    }
}