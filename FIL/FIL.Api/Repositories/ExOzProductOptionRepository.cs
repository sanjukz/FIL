using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzProductOptionRepository : IOrmRepository<ExOzProductOption, ExOzProductOption>
    {
        ExOzProductOption Get(long id);

        ExOzProductOption GetByName(string name);

        List<ExOzProductOption> DisableAllExOzProductOptions();

        IEnumerable<ExOzProductOption> GetByOptionIds(List<long> optionIds);

        ExOzProductOption GetByOptionId(long optionId);
    }

    public class ExOzProductOptionRepository : BaseLongOrmRepository<ExOzProductOption>, IExOzProductOptionRepository
    {
        public ExOzProductOptionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzProductOption Get(long id)
        {
            return Get(new ExOzProductOption { Id = id });
        }

        public IEnumerable<ExOzProductOption> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteProductOption(ExOzProductOption exOzProductOption)
        {
            Delete(exOzProductOption);
        }

        public ExOzProductOption SaveProductOption(ExOzProductOption exOzProductOption)
        {
            return Save(exOzProductOption);
        }

        public ExOzProductOption GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProductOption.Name):C}=@Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public List<ExOzProductOption> DisableAllExOzProductOptions()
        {
            List<ExOzProductOption> allExOzProductOptions = this.GetAll().ToList();
            foreach (var op in allExOzProductOptions)
            {
                op.IsEnabled = false;
                Save(op);
            }
            return allExOzProductOptions;
        }

        public IEnumerable<ExOzProductOption> GetByOptionIds(List<long> optionIds)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProductOption.ProductOptionId):C}IN @OptionIds")
                 .WithParameters(new { OptionIds = optionIds })
             );
        }

        public ExOzProductOption GetByOptionId(long optionId)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProductOption.ProductOptionId):C} = @OptionId")
                .WithParameters(new { OptionId = optionId })
            ).FirstOrDefault();
        }
    }
}