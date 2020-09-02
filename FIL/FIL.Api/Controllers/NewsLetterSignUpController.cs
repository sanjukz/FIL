using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers
{
    public class NewsLetterSignUpController : Controller
    {
        private readonly INewsLetterSignUpRepository _newsLetterSignUpRepository;

        public NewsLetterSignUpController(INewsLetterSignUpRepository newsLetterSignUpRepository)
        {
            _newsLetterSignUpRepository = newsLetterSignUpRepository;
        }

        [HttpGet]
        [Route("api/NewsLetterSignUp/all")]
        public IEnumerable<NewsLetterSignUp> GetAll()
        {
            return _newsLetterSignUpRepository.GetAll();
        }

        [HttpGet]
        [Route("api/NewsLetterSignUp/get/{id}")]
        public NewsLetterSignUp Get(int id)
        {
            return _newsLetterSignUpRepository.Get(id);
        }

        [HttpPost]
        [Route("api/NewsLetterSignUp/save")]
        public NewsLetterSignUp Save([FromBody] NewsLetterSignUp newsLetterSignUp)
        {
            return _newsLetterSignUpRepository.Save(newsLetterSignUp);
        }

        [HttpPost]
        [Route("api/NewsLetterSignUp/delete")]
        public void Delete(NewsLetterSignUp newsLetterSignUp)
        {
            _newsLetterSignUpRepository.Delete(newsLetterSignUp);
        }
    }
}