using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IBoxofficeUserAdditionalDetailRepository : IOrmRepository<BoxofficeUserAdditionalDetail, BoxofficeUserAdditionalDetail>
    {
        BoxofficeUserAdditionalDetail Get(long id);

        BoxofficeUserAdditionalDetail GetByUserId(long userId);

        IEnumerable<BoxofficeUserAdditionalDetail> GetAllById(long parentId);

        IEnumerable<BoxofficeUserAdditionalDetail> GetByUserIdsAndUserType(IEnumerable<long> userIds, int userType);
    }

    public class BoxofficeUserAdditionalDetailRepository : BaseLongOrmRepository<BoxofficeUserAdditionalDetail>, IBoxofficeUserAdditionalDetailRepository
    {
        public BoxofficeUserAdditionalDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public BoxofficeUserAdditionalDetail Get(long id)
        {
            return Get(new BoxofficeUserAdditionalDetail { Id = id });
        }

        public IEnumerable<BoxofficeUserAdditionalDetail> GetAll()
        {
            return GetAll(null);
        }

        public BoxofficeUserAdditionalDetail GetByUserId(long userId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(BoxofficeUserAdditionalDetail.UserId):C} = @UserId")
                .WithParameters(new { UserId = userId })).FirstOrDefault();
        }

        public IEnumerable<BoxofficeUserAdditionalDetail> GetAllById(long parentId)
        {
            var UserList = GetAll(statement => statement
                                 .Where($"{nameof(BoxofficeUserAdditionalDetail.ParentId):C} = @ParentId")
                                 .WithParameters(new { ParentId = parentId }));
            return UserList;
        }

        public IEnumerable<BoxofficeUserAdditionalDetail> GetByUserIdsAndUserType(IEnumerable<long> userIds, int userType)
        {
            var UserList = GetAll(statement => statement
                                 .Where($"{nameof(BoxofficeUserAdditionalDetail.UserId):C} IN @Ids AND UserType=@UserType")
                                 .WithParameters(new { Ids = userIds, UserType = userType }));
            return UserList;
        }
    }
}