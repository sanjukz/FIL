using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IUserCardDetailRepository : IOrmRepository<UserCardDetail, UserCardDetail>
    {
        UserCardDetail Get(long id);

        UserCardDetail GetByAltId(Guid altId);

        IEnumerable<UserCardDetail> GetByUserId(long userId);

        UserCardDetail GetByUserCardDetailId(long? UserCardDetailId);

        IEnumerable<UserCardDetail> GetByCardNumber(string cardNumber);

        UserCardDetail GetByUserCardNumber(string cardNumber, long userId);

        IEnumerable<UserCardDetail> GetByIds(IEnumerable<long?> UserCardDetailIds);
    }

    public class UserCardDetailRepository : BaseLongOrmRepository<UserCardDetail>, IUserCardDetailRepository
    {
        public UserCardDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public UserCardDetail Get(long id)
        {
            return Get(new UserCardDetail { Id = id });
        }

        public IEnumerable<UserCardDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteUserCardDetail(UserCardDetail userCardDetail)
        {
            Delete(userCardDetail);
        }

        public UserCardDetail SaveUserCardDetail(UserCardDetail userCardDetail)
        {
            return Save(userCardDetail);
        }

        public UserCardDetail GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(UserCardDetail.AltId):C}=@AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public IEnumerable<UserCardDetail> GetByUserId(long userId)
        {
            return GetAll(s => s.Where($"{nameof(UserCardDetail.UserId):C}=@UserId")
                .WithParameters(new { UserId = userId }));
        }

        public UserCardDetail GetByUserCardDetailId(long? UserCardDetailId)
        {
            return GetAll(s => s.Where($"{nameof(UserCardDetail.Id):C}=@userCardDetailId")
                .WithParameters(new { userCardDetailId = UserCardDetailId })
            ).FirstOrDefault();
        }

        public IEnumerable<UserCardDetail> GetByCardNumber(string cardNumber)
        {
            return GetAll(s => s.Where($"{nameof(UserCardDetail.CardNumber):C}=@CardNumber")
                .WithParameters(new { CardNumber = cardNumber })
            );
        }

        public UserCardDetail GetByUserCardNumber(string cardNumber, long userId)
        {
            return GetAll(s => s.Where($"{nameof(UserCardDetail.CardNumber):C}=@CardNumber AND {nameof(UserCardDetail.UserId):C}= @UserId")
                .WithParameters(new { CardNumber = cardNumber, UserId = userId })
            ).FirstOrDefault();
        }

        public IEnumerable<UserCardDetail> GetByIds(IEnumerable<long?> UserCardDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(UserCardDetail.Id):C} IN @UserCardDetailIds")
                .WithParameters(new { UserCardDetailIds = UserCardDetailIds })
            );
        }
    }
}