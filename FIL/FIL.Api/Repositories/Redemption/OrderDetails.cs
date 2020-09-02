using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Redemption
{
    public interface IOrderDetailsRepository : IOrmRepository<Redemption_OrderDetails, Redemption_OrderDetails>
    {
        Redemption_OrderDetails Get(int Id);

        Redemption_OrderDetails GetTransaction(long TransactionId);
    }

    public class OrderDetailsRepository : BaseLongOrmRepository<Redemption_OrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Redemption_OrderDetails Get(int Id)
        {
            return Get(new Redemption_OrderDetails { Id = Id });
        }

        public IEnumerable<Redemption_OrderDetails> GetAll()
        {
            return GetAll(null);
        }

        public Redemption_OrderDetails GetTransaction(long TransactionId)
        {
            var OrderDetails = GetAll(
                s => s.Where($"{nameof(Redemption_OrderDetails.TransactionId):C} = @TransactionId")
                .WithParameters(new { TransactionId = TransactionId })).FirstOrDefault();
            return OrderDetails;
        }
    }
}