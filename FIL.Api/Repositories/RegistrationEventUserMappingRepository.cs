using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IRegistrationEventUserMappingRepository : IOrmRepository<RegistrationEventUserMapping, RegistrationEventUserMapping>
    {
        RegistrationEventUserMapping Get(int id);

        RegistrationEventUserMapping GetByEmail(string email);
    }

    public class RegistrationEventUserMappingRepository : BaseLongOrmRepository<RegistrationEventUserMapping>, IRegistrationEventUserMappingRepository
    {
        public RegistrationEventUserMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public RegistrationEventUserMapping Get(int id)
        {
            return Get(new RegistrationEventUserMapping { Id = id });
        }

        public IEnumerable<RegistrationEventUserMapping> GetAll()
        {
            return GetAll(null);
        }

        public RegistrationEventUserMapping GetByEmail(string email)
        {
            return GetAll(s => s.Where($"{nameof(RegistrationEventUserMapping.Email):C} like '%'+@Email+'%'")
           .WithParameters(new { Email = email })).FirstOrDefault(); ;
        }
    }
}