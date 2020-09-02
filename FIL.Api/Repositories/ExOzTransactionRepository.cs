using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IExOzTransactionRepository : IOrmRepository<ExOzTransaction, ExOzTransaction>
    {
        ExOzTransaction Get(int id);
    }

    public class ExOzTransactionRepository : BaseOrmRepository<ExOzTransaction>, IExOzTransactionRepository
    {
        public ExOzTransactionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzTransaction Get(int id)
        {
            return Get(new ExOzTransaction { Id = id });
        }

        public IEnumerable<ExOzTransaction> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteTransaction(ExOzTransaction exOzTransaction)
        {
            Delete(exOzTransaction);
        }

        public ExOzTransaction SaveTransaction(ExOzTransaction exOzTransaction)
        {
            return Save(exOzTransaction);
        }
    }
}