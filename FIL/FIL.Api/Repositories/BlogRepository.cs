using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IBlogRepository : IOrmRepository<Blog, Blog>
    {
        Blog Get(long id);

        Blog GetByBlogId(int id);

        IEnumerable<Blog> GetAll();
    }

    public class BlogRepository : BaseLongOrmRepository<Blog>, IBlogRepository
    {
        public BlogRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public Blog Get(long id)
        {
            return Get(new Blog { Id = id });
        }

        public Blog GetByBlogId(int id)
        {
            return GetAll(s => s.Where($"{nameof(Blog.BlogId):C} = @Id")
                  .WithParameters(new { Id = id })).FirstOrDefault();
        }

        public IEnumerable<Blog> GetAll()
        {
            return GetAll(statement => statement
                .Where($"{nameof(Blog.IsEnabled):C}=1"));
        }
    }
}