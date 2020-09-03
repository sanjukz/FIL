using System;
using System.Threading.Tasks;
using FIL.Contracts.Queries.Creator;
using FIL.Foundation.Senders;
using FIL.Web.Admin.ViewModels.Creator;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Admin.Controllers
{
  public class MyFeelController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;

    public MyFeelController(ICommandSender commandSender, IQuerySender querySender)
    {
      _commandSender = commandSender;
      _querySender = querySender;
    }

    [HttpGet]
    [Route("api/myfeel/{userAltId}")]
    public async Task<MyFeelsViewModel> ApprovePlace(Guid userAltId, bool isApproveModerateScreen, bool isActive)
    {
      try
      {
        var queryResult = await _querySender.Send(new MyFeelsQuery
        {
          CreatedBy = userAltId,
          IsApproveModerateScreen = isApproveModerateScreen,
          IsActive = isActive
        });

        return new MyFeelsViewModel
        {
          Success = queryResult.Success,
          MyFeels = queryResult.MyFeels
        };
      }
      catch (Exception e)
      {
        return new MyFeelsViewModel { };
      }

    }
  }
}
