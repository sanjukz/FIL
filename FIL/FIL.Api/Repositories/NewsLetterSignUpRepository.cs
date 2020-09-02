using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface INewsLetterSignUpRepository : IOrmRepository<NewsLetterSignUp, NewsLetterSignUp>
    {
        NewsLetterSignUp Get(int id);

        NewsLetterSignUp GetByEmail(string email);

        NewsLetterSignUp GetByEmailFeel(string email, bool isFeel);
    }

    public class NewsLetterSignUpRepository : BaseOrmRepository<NewsLetterSignUp>, INewsLetterSignUpRepository
    {
        public NewsLetterSignUpRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public NewsLetterSignUp Get(int id)
        {
            return Get(new NewsLetterSignUp { Id = id });
        }

        public IEnumerable<NewsLetterSignUp> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteNewsLetterSignUp(NewsLetterSignUp newslettersignup)
        {
            Delete(newslettersignup);
        }

        public NewsLetterSignUp SaveNewsLetterSignUp(NewsLetterSignUp newslettersignup)
        {
            return Save(newslettersignup);
        }

        public NewsLetterSignUp GetByEmail(string email)
        {
            return GetAll(s => s.Where($"{nameof(NewsLetterSignUp.Email):C}=@Email")
                .WithParameters(new { Email = email })
            ).FirstOrDefault();
        }

        public NewsLetterSignUp GetByEmailFeel(string email, bool isFeel)
        {
            return GetAll(s => s.Where($"{nameof(NewsLetterSignUp.Email):C}=@Email AND {nameof(NewsLetterSignUp.IsFeel):c}=@IsFeel")
                .WithParameters(new { Email = email, IsFeel = isFeel })
            ).FirstOrDefault();
        }
    }
}