using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzOperatorImageRepository : IOrmRepository<ExOzOperatorImage, ExOzOperatorImage>
    {
        ExOzOperatorImage Get(int id);

        List<ExOzOperatorImage> DisableAllExOzOperatorImages();

        IEnumerable<ExOzOperatorImage> GetByOperatorId(long operatorId);
    }

    public class ExOzOperatorImageRepository : BaseOrmRepository<ExOzOperatorImage>, IExOzOperatorImageRepository
    {
        public ExOzOperatorImageRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzOperatorImage Get(int id)
        {
            return Get(new ExOzOperatorImage { Id = id });
        }

        public IEnumerable<ExOzOperatorImage> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteOperatorImage(ExOzOperatorImage exOzOperatorImage)
        {
            Delete(exOzOperatorImage);
        }

        public ExOzOperatorImage SaveOperatorImage(ExOzOperatorImage exOzOperatorImage)
        {
            return Save(exOzOperatorImage);
        }

        public List<ExOzOperatorImage> DisableAllExOzOperatorImages()
        {
            List<ExOzOperatorImage> allExOzOperatorImages = this.GetAll().ToList();
            foreach (var operatorImage in allExOzOperatorImages)
            {
                operatorImage.IsEnabled = false;
                Save(operatorImage);
            }
            return allExOzOperatorImages;
        }

        public IEnumerable<ExOzOperatorImage> GetByOperatorId(long operatorId)
        {
            return GetAll(s => s.Where($"{nameof(ExOzOperatorImage.OperatorId):C} = @OperatorId")
                .WithParameters(new { OperatorId = operatorId })
            );
        }
    }
}