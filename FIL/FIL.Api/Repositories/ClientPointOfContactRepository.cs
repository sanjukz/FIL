using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IClientPointOfContactRepository : IOrmRepository<ClientPointOfContact, ClientPointOfContact>
    {
        ClientPointOfContact Get(int id);
    }

    public class ClientPointOfContactRepository : BaseOrmRepository<ClientPointOfContact>, IClientPointOfContactRepository
    {
        public ClientPointOfContactRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ClientPointOfContact Get(int id)
        {
            return Get(new ClientPointOfContact { Id = id });
        }

        public IEnumerable<ClientPointOfContact> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteClientPointOfContact(ClientPointOfContact clientPointOfContact)
        {
            Delete(clientPointOfContact);
        }

        public ClientPointOfContact SaveClientPointOfContact(ClientPointOfContact clientPointOfContact)
        {
            return Save(clientPointOfContact);
        }
    }
}