using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IContactUsDetailRepository : IOrmRepository<ContactUsDetail, ContactUsDetail>
    {
        ContactUsDetail Get(int id);
    }

    public class ContactUsDetailRepository : BaseOrmRepository<ContactUsDetail>, IContactUsDetailRepository
    {
        public ContactUsDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ContactUsDetail Get(int id)
        {
            return Get(new ContactUsDetail { Id = id });
        }

        public IEnumerable<ContactUsDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}