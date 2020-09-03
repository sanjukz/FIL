using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FIL.Web.Admin.ViewModels.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Web.Admin.ViewModels;
using FIL.Contracts.Enums;
using Microsoft.AspNetCore.Identity;
using FIL.Web.Core.Helpers;
using FIL.Web.Core.Providers;
using FIL.Web.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using FIL.Contracts.Queries.FeelAdminPlaces;
using System.Text;
using System.Security.Cryptography;

namespace FIL.Web.Admin.Controllers
{
  public class SessionController : Controller
  {
    private readonly IAuthenticationHelper _authenticationHelper;
    private readonly ISessionProvider _sessionProvider;
    private readonly IQuerySender _querySender;
    private readonly IPasswordHasher<string> _passwordHasher;

    public SessionController(IAuthenticationHelper authenticationHelper,
        IPasswordHasher<string> passwordHasher,
        IQuerySender querySender,
        ISessionProvider sessionProvider)
    {
      _authenticationHelper = authenticationHelper;
      _passwordHasher = passwordHasher;
      _sessionProvider = sessionProvider;
      _querySender = querySender;
    }

    [HttpGet]
    [Route("api/session")]
    public async Task<SessionViewModel> Get()
    {
      var data = await _sessionProvider.Get();
      if (data.IsAuthenticated && data.User != null)
      {
        var intercomHash = GetIntercomHash(data.User.AltId);
        data.IntercomHash = intercomHash;
      }
      return data;
    }

    [HttpPost]
    [Route("api/session/login")]
    public async Task<LoginResponseViewModel> Login([FromBody] LoginFormDataViewModel viewModel)
    {

      var passwordHasher = new PasswordHasher<string>();
      var PasswordHash = passwordHasher.HashPassword(viewModel.Email, viewModel.Password);

      var authenticated = false;
      //read cookie from Request object  
      //var token = Request.Cookies["crfu-token"];
      if (viewModel.Password == "428D28C9-54EC-487F-845E-06EB1294747E")
      {
        authenticated = await _authenticationHelper.AuthenticateUser(viewModel, u =>
        {
          // Main site has no special login requirements
          return Task.FromResult(true);
        });
        if (authenticated)
        {
          var Session = await _sessionProvider.Get();
          var queryResult = await _querySender.Send(new FeelAdminPlacesQuery
          {
            UserAltId = Session.User.AltId,
            IsFeelExists = true
          });
          Session.IsFeelExists = queryResult.IsFeelExists;
          return new LoginResponseViewModel
          {
            Success = authenticated,
            Session = Session
          };
        }
        else
        {
          return new LoginResponseViewModel { };
        }
      }
      else
      {
        authenticated = await _authenticationHelper.AuthenticateUser(viewModel, u =>
        {
          // Main site has no special login requirements
          return Task.FromResult(true);
        });
        return new LoginResponseViewModel
        {
          Success = authenticated,
          Session = await _sessionProvider.Get()
        };
      }
    }

    [HttpGet]
    [Route("api/session/logout")]
    public Task<RedirectResult> Logout()
    {
      return _authenticationHelper.SignOut();
    }

    private String GetIntercomHash(Guid userAltId)
    {
      // change according to your needs, an UTF8Encoding
      // could be more suitable in certain situations
      var intercomSecret = "d9BG0qyH5SiPUp65MTY9-2s5GmMxOLdSxTSezmdJ";
      ASCIIEncoding encoding = new ASCIIEncoding();

      Byte[] textBytes = encoding.GetBytes(userAltId.ToString());
      Byte[] keyBytes = encoding.GetBytes(intercomSecret);

      Byte[] hashBytes;

      using (HMACSHA256 hash = new HMACSHA256(keyBytes))
        hashBytes = hash.ComputeHash(textBytes);

      return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
  }
}
