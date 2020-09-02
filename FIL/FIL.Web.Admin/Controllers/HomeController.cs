using FIL.Configuration;
using FIL.Web.Core.Controllers;
using FIL.Web.Core.Providers;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Kitms.Feel.Controllers
{
  public class HomeController : BaseHomeController
  {
    public HomeController(ISiteIdProvider siteIdProvider, ISettings settings, IDynamicSourceProvider dynamicSourceProvider)
        : base(siteIdProvider, settings, dynamicSourceProvider)
    { }

    public override IActionResult Index(Contracts.Enums.Site? siteId)
    {
      siteId = siteId ?? _siteIdProvider.GetSiteId();
      return base.Index(siteId);
    }
  }
}

