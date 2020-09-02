using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IFeelBarcodeMappingRepository : IOrmRepository<FeelBarcodeMapping, FeelBarcodeMapping>
    {
        FeelBarcodeMapping Get(int id);

        FeelBarcodeMapping GetByTransactionDetailId(long TransactionDetailId);

        IEnumerable<FeelBarcodeMapping> GetByTransactionDetailIds(IEnumerable<long> TransactionDetailIds);
    }

    public class FeelBarcodeMappingRepository : BaseLongOrmRepository<FeelBarcodeMapping>, IFeelBarcodeMappingRepository
    {
        public FeelBarcodeMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public FeelBarcodeMapping Get(int id)
        {
            return Get(new FeelBarcodeMapping { Id = id });
        }

        public IEnumerable<FeelBarcodeMapping> GetAll()
        {
            return GetAll(null);
        }

        public FeelBarcodeMapping GetByTransactionDetailId(long TransactionDetailId)
        {
            return GetAll(s => s.Where($"{nameof(FeelBarcodeMapping.TransactionDetailId):C} = @TransactionDetailId")
            .WithParameters(new { TransactionDetailId = TransactionDetailId })
            ).FirstOrDefault();
        }

        public IEnumerable<FeelBarcodeMapping> GetByTransactionDetailIds(IEnumerable<long> TransactionDetailIds)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(FeelBarcodeMapping.TransactionDetailId):C} IN @Ids")
                    .WithParameters(new { Ids = TransactionDetailIds }));
        }
    }
}