using FluentValidation;
using FIL.Contracts.Models;
using FIL.Http;
using FIL.Http.Repositories;
using System;
using System.Threading.Tasks;

namespace FIL.Foundation.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(Guid altId);
    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IRestHelper restHelper)
            : base(restHelper)
        {
        }

        public UserRepository(IRestHelper restHelper, AbstractValidator<User> validator)
            : base(restHelper, validator)
        {
        }

        public Task<User> Get(Guid altId)
        {
            return GetResult($"api/user/{altId}");
        }
    }
}