namespace FIL.Api.Repositories
{
    //public interface IExOzCategoryRepository : IOrmRepository<ExOzCategory, ExOzCategory>
    //{
    //    ExOzCategory Get(int id);
    //    ExOzCategory GetByName(string name);
    //}

    //public class ExOzCategoryRepository : BaseOrmRepository<ExOzCategory>, IExOzCategoryRepository
    //{
    //    public ExOzCategoryRepository(IDataSettings dataSettings) : base(dataSettings)
    //    {
    //    }

    //    public ExOzCategory Get(int id)
    //    {
    //        return Get(new ExOzCategory { Id = id });
    //    }

    //    public IEnumerable<ExOzCategory> GetAll()
    //    {
    //        return GetAll(null);
    //    }

    //    public void DeleteCategory(ExOzCategory exOzCategory)
    //    {
    //        Delete(exOzCategory);
    //    }

    //    public ExOzCategory SaveCategory(ExOzCategory exOzCategory)
    //    {
    //        return Save(exOzCategory);
    //    }

    //    public ExOzCategory GetByName(string name)
    //    {
    //        return GetAll(s => s.Where($"{nameof(ExOzCategory.Name):C}=@Name")
    //            .WithParameters(new { Name = name })
    //        ).FirstOrDefault();
    //    }
    //}
}