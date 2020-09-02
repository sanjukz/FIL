using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITicketRefundDetailRepository : IOrmRepository<TicketRefundDetail, TicketRefundDetail>
    {
        TicketRefundDetail Get(long id);

        TicketRefundDetail GetByBarcodeNumber(string barcodeNumber);
    }

    public class TicketRefundDetailRepository : BaseLongOrmRepository<TicketRefundDetail>, ITicketRefundDetailRepository
    {
        public TicketRefundDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TicketRefundDetail Get(long id)
        {
            return Get(new TicketRefundDetail { Id = id });
        }

        public IEnumerable<TicketRefundDetail> GetAll()
        {
            return GetAll(null);
        }

        public TicketRefundDetail GetByBarcodeNumber(string barcodeNumber)
        {
            return GetAll(s => s.Where($"{nameof(TicketRefundDetail.BarcodeNumber):C} = @BarcodeNumber")
            .WithParameters(new { BarcodeNumber = barcodeNumber })
            ).FirstOrDefault();
        }
    }
}