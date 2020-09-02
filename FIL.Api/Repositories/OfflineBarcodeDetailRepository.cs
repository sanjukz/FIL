using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IOfflineBarcodeDetailRepository : IOrmRepository<OfflineBarcodeDetail, OfflineBarcodeDetail>
    {
        OfflineBarcodeDetail Get(long id);

        OfflineBarcodeDetail GetByBarcodeNumber(string barcodenumber);

        OfflineBarcodeDetail GetByBarcodeNumberAndEventTicketDetailIds(string barcodenumber, IEnumerable<long> eventTicketDetailIds);
    }

    public class OfflineBarcodeDetailRepository : BaseLongOrmRepository<OfflineBarcodeDetail>, IOfflineBarcodeDetailRepository
    {
        public OfflineBarcodeDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public OfflineBarcodeDetail Get(long id)
        {
            return Get(new OfflineBarcodeDetail { Id = id });
        }

        public IEnumerable<OfflineBarcodeDetail> GetAll()
        {
            return GetAll(null);
        }

        public OfflineBarcodeDetail GetByBarcodeNumberAndEventTicketDetailIds(string barcodenumber, IEnumerable<long> eventTicketDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(OfflineBarcodeDetail.BarcodeNumber):C} = @BarcodeNumber AND {nameof(MatchSeatTicketDetail.EventTicketDetailId):C} IN @EventTicketDetailIds ")
                .WithParameters(new { BarcodeNumber = barcodenumber, EventTicketDetailIds = eventTicketDetailIds })
            ).FirstOrDefault();
        }

        public OfflineBarcodeDetail GetByBarcodeNumber(string barcodenumber)
        {
            return GetAll(s => s.Where($"{nameof(OfflineBarcodeDetail.BarcodeNumber):C} = @BarcodeNumber")
                .WithParameters(new { BarcodeNumber = barcodenumber })
            ).FirstOrDefault();
        }
    }
}