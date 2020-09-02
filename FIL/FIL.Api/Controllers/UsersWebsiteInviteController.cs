using FIL.Api.Repositories;
using FIL.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers
{
    public class UsersWebsiteInviteController : Controller
    {
        private readonly IUsersWebsiteInviteRepository _usersWebsiteInviteRepository;

        public UsersWebsiteInviteController(IUsersWebsiteInviteRepository usersWebsiteInviteRepository)
        {
            _usersWebsiteInviteRepository = usersWebsiteInviteRepository;
        }

        [HttpGet]
        [Route("api/invite/all")]
        public IEnumerable<UsersWebsiteInvite> GetAll()
        {
            return _usersWebsiteInviteRepository.GetAll();
        }

        [HttpGet]
        public UsersWebsiteInvite Get(int id)
        {
            return _usersWebsiteInviteRepository.Get(id);
        }

        [HttpPost]
        public UsersWebsiteInvite Save(UsersWebsiteInvite invite)
        {
            return _usersWebsiteInviteRepository.Save(invite);
        }

        [HttpPost]
        [Route("api/stainvitete/delete")]
        public void Delete(UsersWebsiteInvite invite)
        {
            _usersWebsiteInviteRepository.Delete(invite);
        }
    }
}