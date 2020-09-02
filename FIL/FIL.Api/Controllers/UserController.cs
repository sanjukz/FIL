using AutoMapper;
using FIL.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FIL.Api.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("api/user/{altId}")]
        public Contracts.Models.User Get(Guid altId)
        {
            var user = _userRepository.GetByAltId(altId);
            return user == null ? null : Mapper.Map<Contracts.Models.User>(user);
        }
    }
}