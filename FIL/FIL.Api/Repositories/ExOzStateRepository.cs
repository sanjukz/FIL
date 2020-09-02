using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzStateRepository : IOrmRepository<ExOzState, ExOzState>
    {
        ExOzState Get(int id);

        ExOzState GetByName(string name);

        IEnumerable<ExOzState> GetByNames(List<string> names);

        IEnumerable<ExOzState> GetAll();

        List<ExOzState> DisableAllExOzStates();

        ExOzState GetByUrlSegment(string urlSegment);
    }

    public class ExOzStateRepository : BaseOrmRepository<ExOzState>, IExOzStateRepository
    {
        public ExOzStateRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzState Get(int id)
        {
            return Get(new ExOzState { Id = id });
        }

        public IEnumerable<ExOzState> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteState(ExOzState exOzState)
        {
            Delete(exOzState);
        }

        public ExOzState SaveState(ExOzState exOzState)
        {
            return Save(exOzState);
        }

        public ExOzState GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ExOzState.Name):C}=@Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public IEnumerable<ExOzState> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(ExOzState.Name):C} in @Name")
                .WithParameters(new { Name = names })
            );
        }

        public List<ExOzState> DisableAllExOzStates()
        {
            List<ExOzState> allExOzStates = this.GetAll().ToList();
            foreach (var state in allExOzStates)
            {
                state.IsEnabled = false;
                Save(state);
            }
            return allExOzStates;
        }

        public ExOzState GetByUrlSegment(string urlSegment)
        {
            return GetAll(s => s.Where($"{nameof(ExOzState.UrlSegment):C}=@UrlSegment")
                .WithParameters(new { UrlSegment = urlSegment })
            ).FirstOrDefault();
        }
    }
}