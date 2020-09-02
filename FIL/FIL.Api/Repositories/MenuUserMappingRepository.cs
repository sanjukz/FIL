using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IMenuUserMappingRepository : IOrmRepository<MenuUserMapping, MenuUserMapping>
    {
        MenuUserMapping Get(int id);
    }

    public class MenuUserMappingRepository : BaseOrmRepository<MenuUserMapping>, IMenuUserMappingRepository
    {
        public MenuUserMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MenuUserMapping Get(int id)
        {
            return Get(new MenuUserMapping { Id = id });
        }

        public IEnumerable<MenuUserMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteMenuUserMapping(MenuUserMapping menuUserMapping)
        {
            Delete(menuUserMapping);
        }

        public MenuUserMapping SaveMenuUserMapping(MenuUserMapping menuUserMapping)
        {
            return Save(menuUserMapping);
        }
    }
}