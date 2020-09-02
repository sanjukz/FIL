using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IMenuRepository : IOrmRepository<Menu, Menu>
    {
        Menu Get(int id);
    }

    public class MenuRepository : BaseOrmRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Menu Get(int id)
        {
            return Get(new Menu { Id = id });
        }

        public IEnumerable<Menu> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteMenu(Menu menu)
        {
            Delete(menu);
        }

        public Menu SaveMenu(Menu menu)
        {
            return Save(menu);
        }
    }
}