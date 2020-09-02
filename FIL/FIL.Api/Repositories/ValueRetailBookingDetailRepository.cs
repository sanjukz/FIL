using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IValueRetailBookingDetailRepository : IOrmRepository<ValueRetailBookingDetail, ValueRetailBookingDetail>
    {
        ValueRetailBookingDetail Get(int id);

        ValueRetailBookingDetail GetByEmail(string email);

        ValueRetailBookingDetail GetByJobId(int jobId);
    }

    public class ValueRetailBookingDetailRepository : BaseOrmRepository<ValueRetailBookingDetail>, IValueRetailBookingDetailRepository
    {
        public ValueRetailBookingDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ValueRetailBookingDetail Get(int id)
        {
            return Get(new ValueRetailBookingDetail { Id = id });
        }

        public ValueRetailBookingDetail GetByEmail(string email)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailBookingDetail.Email):C} = @EmailParam").WithParameters(new { EmailParam = email })).FirstOrDefault();
        }

        public ValueRetailBookingDetail GetByJobId(int jobId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailBookingDetail.JobId):C} = @JobIdParam").WithParameters(new { JobIdParam = jobId })).FirstOrDefault();
        }

        public IEnumerable<ValueRetailBookingDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteValueRetailBookingDetail(ValueRetailBookingDetail valueRetailBookingDetail)
        {
            Delete(valueRetailBookingDetail);
        }

        public ValueRetailBookingDetail SaveValueRetailBookingDetail(ValueRetailBookingDetail valueRetailBookingDetail)
        {
            return Save(valueRetailBookingDetail);
        }
    }
}