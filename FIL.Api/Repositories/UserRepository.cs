using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IUserRepository : IOrmRepository<User, User>
    {
        User Get(long id);

        User GetByEmail(string email);

        User GetByEmailAndChannel(string email, Channels? channelId, SignUpMethods? SignUpMethodId);

        User GetByPhoneAndChannel(string phoneCode, string phoneNumber, Channels? channelId);

        User GetByAltId(Guid altId);

        IEnumerable<User> GetByAltIds(IEnumerable<Guid> altIds);

        IEnumerable<User> GetByRoleIdAndChannel(int roleId, Channels? channelId);

        IEnumerable<User> GetAllByAltIds(IEnumerable<Guid> altIds);

        IEnumerable<User> GetById(Guid id);

        IEnumerable<User> GetAllByChannel(Channels? channelId);

        IEnumerable<User> GetByUserIds(IEnumerable<long> userIds);

        User GetBySocialIdAndChannel(string socialId, Channels? channelId);

        User GetByEmailIdAndChannelId(string email, Channels? channelId);
    }

    public class UserRepository : BaseLongOrmRepository<User>, IUserRepository
    {
        public UserRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public User Get(long id)
        {
            return Get(new User { Id = id });
        }

        public IEnumerable<User> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<User> GetByUserIds(IEnumerable<long> userIds)
        {
            var UserList = GetAll(statement => statement
                                 .Where($"{nameof(User.Id):C} IN @Ids")
                                 .WithParameters(new { Ids = userIds }));
            return UserList;
        }

        public User GetByEmail(string email)
        {
            return GetAll(s => s.Where($"{nameof(User.Email):C} = @Email")
                .WithParameters(new { Email = email })
            ).FirstOrDefault();
        }

        public User GetByEmailAndChannel(string email, Channels? channelId, SignUpMethods? signUpMethodId)
        {
            if (signUpMethodId == null)
            {
                return GetAll(s => s.Where($"{nameof(User.Email):C}=@Email AND {nameof(User.ChannelId):C}=@ChannelId")
                    .WithParameters(new { Email = email, ChannelId = channelId })
            ).FirstOrDefault();
            }
            else
            {
                return GetAll(s => s.Where($"{nameof(User.Email):C}=@Email AND {nameof(User.ChannelId):C}=@ChannelId AND { nameof(User.SignUpMethodId):C}= @SignUpMethodId")
                    .WithParameters(new { Email = email, ChannelId = channelId, SignUpMethodId = signUpMethodId })
            ).FirstOrDefault();
            }
        }

        public User GetByPhoneAndChannel(string phoneCode, string phoneNumber, Channels? channelId)
        {
            return GetAll(s => s.Where($"{nameof(User.PhoneCode):C}=@PhoneCode AND {nameof(User.ChannelId):C}=@ChannelId AND { nameof(User.PhoneNumber):C}= @PhoneNumber")
             .WithParameters(new { PhoneCode = phoneCode, ChannelId = channelId, PhoneNumber = phoneNumber })).FirstOrDefault();
        }

        public User GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(User.AltId):C} = @AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public IEnumerable<User> GetByRoleIdAndChannel(int roleId, Channels? channelId)
        {
            return GetAll(s => s.Where($"{nameof(User.RolesId):C} = @RoleId  AND {nameof(User.ChannelId):C}=@ChannelId")
           .WithParameters(new { RoleId = roleId, ChannelId = channelId }));
        }

        public IEnumerable<User> GetByAltIds(IEnumerable<Guid> altIds)
        {
            return GetAll(s => s.Where($"{nameof(User.AltId):C} IN @AltIds")
               .WithParameters(new { AltIds = altIds }));
        }

        public IEnumerable<User> GetAllByAltIds(IEnumerable<Guid> altIds)
        {
            return GetAll(s => s.Where($"{nameof(User.AltId):C} IN @AltIds")
               .WithParameters(new { AltIds = altIds }));
        }

        public IEnumerable<User> GetAllByChannel(Channels? channelId)
        {
            return GetAll(s => s.Where($"{nameof(User.ChannelId):C} = @ChannelId")
            .WithParameters(new { ChannelId = channelId }));
        }

        public IEnumerable<User> GetById(Guid id)
        {
            return GetAll(s => s.Where($"{nameof(User.AltId):C} = @Id")
            .WithParameters(new { Id = id }));
        }

        public User GetBySocialIdAndChannel(string socialId, Channels? channelId)
        {
            return GetAll(s => s.Where($"{nameof(User.SocialLoginId):C} = @SocialId  AND {nameof(User.ChannelId):C}=@ChannelId")
          .WithParameters(new { SocialId = socialId, ChannelId = channelId })).FirstOrDefault();
        }

        public User GetByEmailIdAndChannelId(string email, Channels? channelId)
        {
            return GetAll(s => s.Where($"{nameof(User.Email):C} = @Email AND {nameof(User.ChannelId):C}=@ChannelId")
                .WithParameters(new { Email = email, ChannelId = channelId })
            ).FirstOrDefault();
        }
    }
}