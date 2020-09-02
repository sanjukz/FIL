using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IZipcodeRepository : IOrmRepository<Zipcode, Zipcode>
    {
        Zipcode Get(int id);

        Zipcode GetByZipcode(string zipcode);

        Zipcode GetByAltId(Guid altId);

        IEnumerable<Zipcode> GetAllByCityId(int cityId);
    }

    public class ZipcodeRepository : BaseOrmRepository<Zipcode>, IZipcodeRepository
    {
        public ZipcodeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Zipcode Get(int id)
        {
            return Get(new Zipcode { Id = id });
        }

        public IEnumerable<Zipcode> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<Zipcode> GetAllByCityId(int cityId)
        {
            return GetAll(s => s.Where($"{nameof(Zipcode.CityId):C}=@Id")
                  .WithParameters(new { Id = cityId }));
        }

        public void DeleteZipcode(Zipcode zipcode)
        {
            Delete(zipcode);
        }

        public Zipcode SaveZipcode(Zipcode zipcode)
        {
            return Save(zipcode);
        }

        public Zipcode GetByZipcode(string postalCode)
        {
            return GetAll(s => s.Where($"{nameof(Zipcode.Postalcode):C}=@Postalcode")
                .WithParameters(new { Postalcode = postalCode })
            ).FirstOrDefault();
        }

        public Zipcode GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(Zipcode.AltId):C}=@AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }
    }
}