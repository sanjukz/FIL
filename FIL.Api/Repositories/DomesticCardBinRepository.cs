using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IDomesticCardBinRepository : IOrmRepository<DomesticCardBin, DomesticCardBin>
    {
        DomesticCardBin Get(int id);
    }

    public class DomesticCardBinRepository : BaseOrmRepository<DomesticCardBin>, IDomesticCardBinRepository
    {
        public DomesticCardBinRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public DomesticCardBin Get(int id)
        {
            return Get(new DomesticCardBin { Id = id });
        }

        public IEnumerable<DomesticCardBin> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteDomesticCardBin(DomesticCardBin domesticCardBin)
        {
            Delete(domesticCardBin);
        }

        public DomesticCardBin SaveDomesticCardBin(DomesticCardBin domesticCardBin)
        {
            return Save(domesticCardBin);
        }
    }
}