using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IUserAddressDetailRepository : IOrmRepository<UserAddressDetail, UserAddressDetail>
    {
        UserAddressDetail Get(int id);

        UserAddressDetail GetByAltId(Guid altId);

        IEnumerable<UserAddressDetail> GetByUserId(long altId);

        UserAddressDetail GetByUser(long userId);
    }

    public class UserAddressDetailRepository : BaseLongOrmRepository<UserAddressDetail>, IUserAddressDetailRepository
    {
        public UserAddressDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public UserAddressDetail Get(int id)
        {
            return Get(new UserAddressDetail { Id = id });
        }

        public IEnumerable<UserAddressDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteUserAddressDetail(UserAddressDetail userAddressDetail)
        {
            Delete(userAddressDetail);
        }

        public UserAddressDetail SaveUserAddressDetail(UserAddressDetail userAddressDetail)
        {
            return Save(userAddressDetail);
        }

        public UserAddressDetail GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(UserAddressDetail.AltId):C}=@AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public IEnumerable<UserAddressDetail> GetByUserId(long userId)
        {
            return GetAll(s => s.Where($"{nameof(UserAddressDetail.UserId):C}=@UserId")
                .WithParameters(new { UserId = userId })
            );
        }

        public UserAddressDetail GetByUser(long userId)
        {
            return GetAll(s => s.Where($"{nameof(UserAddressDetail.UserId):C}=@UserId")
                .WithParameters(new { UserId = userId })
            ).FirstOrDefault();
        }
    }
}