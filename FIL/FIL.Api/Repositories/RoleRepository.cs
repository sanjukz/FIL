using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IRoleRepository : IOrmRepository<Role, Role>
    {
        Role Get(int id);

        IEnumerable<Role> GetAllByModule(int moduleId);

        IEnumerable<Role> GetZsuiteModules();
    }

    public class RoleRepository : BaseOrmRepository<Role>, IRoleRepository
    {
        public RoleRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Role Get(int id)
        {
            return Get(new Role { Id = id });
        }

        public IEnumerable<Role> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<Role> GetAllByModule(int moduleId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(Role.ModuleId):C} = @Id")
                    .WithParameters(new { Id = moduleId }));
        }

        public IEnumerable<Role> GetZsuiteModules()
        {
            return GetAll(statement => statement
                    .Where($"{nameof(Role.ModuleId):C} IN(1,3,6)")
            );
        }

        public void DeleteRole(Role role)
        {
            Delete(role);
        }

        public Role SaveRole(Role role)
        {
            return Save(role);
        }
    }
}