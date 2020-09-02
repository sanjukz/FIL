using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzOperatorRepository : IOrmRepository<ExOzOperator, ExOzOperator>
    {
        ExOzOperator Get(int id);

        ExOzOperator GetByName(string name);

        ExOzOperator GetByUrlSegment(string urlSegment);

        List<ExOzOperator> DisableAllExOzOperators();

        IEnumerable<ExOzOperator> GetByNames(List<string> names);
    }

    public class ExOzOperatorRepository : BaseLongOrmRepository<ExOzOperator>, IExOzOperatorRepository
    {
        public ExOzOperatorRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzOperator Get(int id)
        {
            return Get(new ExOzOperator { Id = id });
        }

        public IEnumerable<ExOzOperator> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteOperator(ExOzOperator exOzOperator)
        {
            Delete(exOzOperator);
        }

        public ExOzOperator SaveOperator(ExOzOperator exOzOperator)
        {
            return Save(exOzOperator);
        }

        public ExOzOperator GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ExOzOperator.Name):C}=@Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public ExOzOperator GetByUrlSegment(string urlSegment)
        {
            return GetAll(s => s.Where($"{nameof(ExOzOperator.UrlSegment):C}=@UrlSegment")
                .WithParameters(new { UrlSegment = urlSegment })
            ).FirstOrDefault();
        }

        public List<ExOzOperator> DisableAllExOzOperators()
        {
            List<ExOzOperator> allExOzOperators = this.GetAll().ToList();
            foreach (var op in allExOzOperators)
            {
                op.IsEnabled = false;
                Save(op);
            }
            return allExOzOperators;
        }

        public IEnumerable<ExOzOperator> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(ExOzOperator.Name):C} in @Name")
                .WithParameters(new { Name = names })
            );
        }
    }
}