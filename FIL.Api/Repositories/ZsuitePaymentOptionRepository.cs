using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IZsuitePaymentOptionRepository : IOrmRepository<ZsuitePaymentOption, ZsuitePaymentOption>
    {
        ZsuitePaymentOption Get(long id);

        IEnumerable<ZsuitePaymentOption> GetAll();
    }

    public class ZsuitePaymentOptionRepository : BaseLongOrmRepository<ZsuitePaymentOption>, IZsuitePaymentOptionRepository
    {
        public ZsuitePaymentOptionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ZsuitePaymentOption Get(long id)
        {
            return Get(new ZsuitePaymentOption { Id = id });
        }

        public IEnumerable<ZsuitePaymentOption> GetAll()
        {
            return GetAll(s => s.Where($"{nameof(ZsuitePaymentOption.IsEnabled):C} =1"));
        }
    }
}