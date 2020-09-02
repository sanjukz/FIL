using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICompanyDetailRepository : IOrmRepository<CompanyDetail, CompanyDetail>
    {
        CompanyDetail Get(int id);
    }

    public class CompanyDetailRepository : BaseOrmRepository<CompanyDetail>, ICompanyDetailRepository
    {
        public CompanyDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CompanyDetail Get(int id)
        {
            return Get(new CompanyDetail { Id = id });
        }

        public IEnumerable<CompanyDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCardDetail(CompanyDetail companyDetail)
        {
            Delete(companyDetail);
        }

        public CompanyDetail SaveCardDetail(CompanyDetail companyDetail)
        {
            return Save(companyDetail);
        }
    }
}