using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FIL.Configuration;
using FIL.Web.Core.Providers;
using FIL.Configuration.Utilities;
using FIL.Contracts.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace FIL.Web.Core.Controllers
{
    public abstract class BaseHomeController : Controller
    {
        private readonly ISettings _settings;
        protected readonly ISiteIdProvider _siteIdProvider;
        private readonly IDynamicSourceProvider _dynamicSourceProvider;
        protected BaseHomeController(
            ISiteIdProvider siteIdProvider,
            ISettings settings, IDynamicSourceProvider dynamicSourceProvider)
        {
            _siteIdProvider = siteIdProvider;
            _settings = settings;
            _dynamicSourceProvider = dynamicSourceProvider;
        }

        public virtual IActionResult Index(Contracts.Enums.Site? siteId)
        {
            var sentryDsn = _settings.ResolveSetting("SENTRY_PUBLIC_DSN", "publicSentryUrl");
            var googleTagManagerAccountId = _settings.ResolveSetting("GTM_ACCOUNT_ID", "gtmAccountId");
            var baseStaticUrl = _settings.ResolveSetting("BASE_STATIC_URL", "baseStaticUrl");
            var stripePublicToken = _settings.ResolveSetting("STRIPE_PUBLIC_TOKEN", "stripePublicToken");
             var staticUrls = _siteIdProvider.GetSiteId() == Site.FeelDevelopmentSite ? _settings.GetConfigSetting(SettingKeys.Aws.S3.PathName).Value + "/" + _settings.GetConfigSetting(SettingKeys.Aws.S3.Feel.BucketName).Value :
               _siteIdProvider.GetSiteId() == Site.feelaplaceSite ? _settings.GetConfigSetting(SettingKeys.Aws.S3.Feel.StaticURL).Value : _siteIdProvider.GetSiteId() == Site.DevelopmentSite ?
                _settings.GetConfigSetting(SettingKeys.Aws.S3.PathName).Value + "/" + _settings.GetConfigSetting(SettingKeys.Aws.S3.Zoonga.BucketName).Value :
                _settings.GetConfigSetting(SettingKeys.Aws.S3.Zoonga.StaticURL).Value;

            var stripeConnectClientId = _settings.GetConfigSetting(SettingKeys.PaymentGateway.Stripe.Connect.ConnectClinetId).Value + "," + _settings.GetConfigSetting(SettingKeys.PaymentGateway.Stripe.Connect.ConnectAustraliaClinetId).Value + "," + _settings.GetConfigSetting(SettingKeys.PaymentGateway.Stripe.Connect.ConnectIndiaClinetId).Value;
            var StripeAustraliaPublicToken = _settings.GetConfigSetting(SettingKeys.PaymentGateway.Stripe.FeelAustralia.PublishableKey).Value;
            var StripeIndiaPublicToken = _settings.GetConfigSetting(SettingKeys.PaymentGateway.Stripe.FeelIndia.PublishableKey).Value;
            // TODO: S3 bucket URLs + static<1-5> CDN urls
            ViewBag.SentryDsn = sentryDsn;
            if (!string.IsNullOrWhiteSpace(googleTagManagerAccountId))
            {
                ViewBag.GoogleTagManagerAccountId = googleTagManagerAccountId;
            }
            if (!string.IsNullOrWhiteSpace(baseStaticUrl))
            {
                ViewBag.BaseStaticUrl = baseStaticUrl;
            }
            if (!string.IsNullOrWhiteSpace(stripePublicToken))
            {
                ViewBag.StripePublicToken = stripePublicToken;
            }
            if (!string.IsNullOrWhiteSpace(staticUrls))
            {
                ViewBag.Urls = staticUrls;
            }
            if (!string.IsNullOrWhiteSpace(stripeConnectClientId))
            {
                ViewBag.stripeConnectClientId = stripeConnectClientId;
            }
            if (!string.IsNullOrWhiteSpace(StripeAustraliaPublicToken))
            {
                ViewBag.StripeAustraliaPublicToken = StripeAustraliaPublicToken;
            }
            if (!string.IsNullOrWhiteSpace(StripeIndiaPublicToken))
            {
                ViewBag.StripeIndiaPublicToken = StripeIndiaPublicToken;
            }
            ViewBag.SiteId = (int)(siteId ?? _siteIdProvider.GetSiteId());
            //code for geo reidrection. enforced only on dev sites

            ViewBag.ShouldIndex = true;
            ViewBag.IsLiveOnline = false;
            ViewBag.isLiveStream = false;
            ViewBag.currentPathUrl = "/";
            //Get content for FeelAPlace
            if (siteId == Site.feelaplaceSite || siteId == Site.FeelDevelopmentSite || siteId == Site.ComSite || siteId == Site.DevelopmentSite)
            {
                Task.WaitAll(_dynamicSourceProvider.GetDynamicContentAsync(ViewBag));
            }
            else
            {
                ViewBag.OneSignalAppId = String.Empty;
            }
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

    }
}
