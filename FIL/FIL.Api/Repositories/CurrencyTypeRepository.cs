using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICurrencyTypeRepository : IOrmRepository<CurrencyType, CurrencyType>
    {
        CurrencyType Get(int id);

        CurrencyType GetByCurrencyId(long id);

        IEnumerable<CurrencyType> GetByCurrencyIds(IEnumerable<int> ids);

        IEnumerable<CurrencyType> GetByCountryIds(IEnumerable<int> ids);

        CurrencyType GetById(int currencyId);

        CurrencyType GetByCurrencyCode(string code);

        CurrencyType GetByCountryId(int currencyId);

        List<CurrencyType> GetByCurrencyIdsByEventIds(string ids);

        IEnumerable<CurrencyType> GetByEventId(long eventId);
    }

    public class CurrencyTypeRepository : BaseOrmRepository<CurrencyType>, ICurrencyTypeRepository
    {
        public CurrencyTypeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CurrencyType Get(int id)
        {
            return Get(new CurrencyType { Id = id });
        }

        public IEnumerable<CurrencyType> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCurrency(CurrencyType currencyType)
        {
            Delete(currencyType);
        }

        public CurrencyType SaveCurrency(CurrencyType currencyType)
        {
            return Save(currencyType);
        }

        public CurrencyType GetByCurrencyId(long id)
        {
            return GetAll(s => s.Where($"{nameof(CurrencyType.Id):C} = @Id")
                .WithParameters(new { Id = id })
            ).FirstOrDefault();
        }

        public CurrencyType GetById(int currencyId)
        {
            return GetAll(s => s.Where($"{nameof(CurrencyType.Id):C} = @CurrencyId")
               .WithParameters(new { CurrencyId = currencyId })
           ).FirstOrDefault();
        }

        public CurrencyType GetByCurrencyCode(string code)
        {
            return GetAll(s => s.Where($"{nameof(CurrencyType.Code):C} = @Code")
               .WithParameters(new { Code = code })
             ).FirstOrDefault();
        }

        public CurrencyType GetByCountryId(int countryId)
        {
            return GetAll(s => s.Where($"{nameof(CurrencyType.CountryId):C} = @CountryId")
               .WithParameters(new { CountryId = countryId })
           ).FirstOrDefault();
        }

        public IEnumerable<CurrencyType> GetByCurrencyIds(IEnumerable<int> ids)
        {
            var currencyTypeList = GetAll(statement => statement
                                    .Where($"{nameof(CurrencyType.Id):C} IN @Ids")
                                    .WithParameters(new { Ids = ids }));
            return currencyTypeList;
        }

        public IEnumerable<CurrencyType> GetByCountryIds(IEnumerable<int> ids)
        {
            var currencyTypeList = GetAll(statement => statement
                                    .Where($"{nameof(CurrencyType.CountryId):C} IN @Ids")
                                    .WithParameters(new { Ids = ids }));
            return currencyTypeList;
        }

        public List<CurrencyType> GetByCurrencyIdsByEventIds(string ids)
        {
            List<CurrencyType> currencyTypes = new List<CurrencyType>();
            var currencyTypeList = GetCurrentConnection().QueryMultiple("spGetEventCurrency", new { EventId = ids }, commandType: CommandType.StoredProcedure);
            currencyTypes = currencyTypeList.Read<CurrencyType>().ToList();
            return currencyTypes;
        }

        public IEnumerable<CurrencyType> GetByEventId(long eventId)
        {
            return GetCurrentConnection().Query<CurrencyType>("SELECT DISTINCT CT.* FROM EventDetails ED WITH(NOLOCK) " +
            "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.EventDetailId = ED.Id " +
            "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id " +
            "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON CT.Id = ETA.CurrencyId " +
            "WHERE ED.EventId=@EventId " +
            "UNION  " +
            "SELECT DISTINCT CT.* FROM EventDetails ED WITH(NOLOCK)  " +
            "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.EventDetailId = ED.Id  " +
            "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id  " +
            "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON CT.Id = ETA.LocalCurrencyId  " +
            "WHERE ED.EventId= @EventId", new
            {
                EventId = eventId
            });
        }
    }
}