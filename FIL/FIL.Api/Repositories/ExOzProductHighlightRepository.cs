using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzProductHighlightRepository : IOrmRepository<ExOzProductHighlight, ExOzProductHighlight>
    {
        ExOzProductHighlight Get(long id);

        List<ExOzProductHighlight> DisableAllExOzProductHighlights();

        IEnumerable<ExOzProductHighlight> GetByProductId(long productId);
    }

    public class ExOzProductHighlightRepository : BaseLongOrmRepository<ExOzProductHighlight>, IExOzProductHighlightRepository
    {
        public ExOzProductHighlightRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzProductHighlight Get(long id)
        {
            return Get(new ExOzProductHighlight { Id = id });
        }

        public IEnumerable<ExOzProductHighlight> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteProductHighlight(ExOzProductHighlight exOzProductHighlight)
        {
            Delete(exOzProductHighlight);
        }

        public ExOzProductHighlight SaveProductHighlight(ExOzProductHighlight exOzProductHighlight)
        {
            return Save(exOzProductHighlight);
        }

        public List<ExOzProductHighlight> DisableAllExOzProductHighlights()
        {
            List<ExOzProductHighlight> allExOzProductHighlights = this.GetAll().ToList();
            foreach (var productHighlight in allExOzProductHighlights)
            {
                productHighlight.IsEnabled = false;
                Save(productHighlight);
            }
            return allExOzProductHighlights;
        }

        public IEnumerable<ExOzProductHighlight> GetByProductId(long productId)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProductHighlight.ProductId):C} = @ProductId")
                .WithParameters(new { ProductId = productId })
            );
        }
    }
}