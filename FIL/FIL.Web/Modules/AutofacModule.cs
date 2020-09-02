using Autofac;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Foundation.Senders;
using FIL.Http;
using FIL.MailChimp;
using FIL.Messaging.Senders;
using FIL.Web.Core;
using FIL.Web.Core.Providers;
using FIL.Web.Feel.Builders;
using FIL.Web.Feel.Modules.SiteExtensions;
using FIL.Web.Feel.Providers;
using FIL.Web.Feel.Providers.Itinerary;
using System;

namespace FIL.Web.Feel.Modules
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfirmationEmailSender>().As<IConfirmationEmailSender>();
            builder.RegisterType<AccountEmailSender>().As<IAccountEmailSender>();
            builder.RegisterType<AmazonS3FileProvider>().As<IAmazonS3FileProvider>();
            builder.RegisterType<SiteIdProvider>().AsImplementedInterfaces();

            //geo site setup Interfaces
            builder.RegisterType<GeoCurrency>().As<IGeoCurrency>();
            builder.RegisterType<GeoRedirection>().As<IGeoRedirection>();
            builder.RegisterType<CurrencyConverter.Currency>().As<CurrencyConverter.ICurrencyConverter>();

            builder.RegisterType<LatLongProvider>().As<ILatLongProvider>();
            builder.RegisterType<VisitDurationProvider>().As<IVisitDurationProvider>();
            builder.RegisterType<NearestPlaceProvider>().As<INearestPlaceProvider>();
            builder.RegisterType<PlacePriceProvider>().As<IPlacePriceProvider>();
            builder.RegisterType<DurationTimeProvider>().As<IDurationTimeProvider>();
            builder.RegisterType<LocalDateTimeProvider>().As<ILocalDateTimeProvider>();
            builder.RegisterType<TwilioTextMessageSender>().As<ITwilioTextMessageSender>();
            builder.RegisterType<GupShupTextMessageSender>().As<IGupShupTextMessageSender>();
            builder.RegisterType<MailChimpProvider>().As<IMailChimpProvider>();

            builder.Register(c => new RestHelper(new Uri(c.Resolve<ISettings>().ResolveSetting("SEARCH_ENDPOINT", "searchEndpoint") ?? c.Resolve<ISettings>().GetConfigSetting<string>(SettingKeys.Integration.Feel.Endpoint))))
                .Keyed<IRestHelper>("SearchRestHelper");

            builder.Register<SearchProvider>(c => new SearchProvider(c.Resolve<ISessionProvider>(),
                c.ResolveKeyed<IRestHelper>("SearchRestHelper"),
                c.Resolve<Logging.ILogger>(),
                c.Resolve<IEventSearchResultBuilder>(),
                c.Resolve<IQuerySender>())).AsImplementedInterfaces();
            builder.RegisterType<EventSearchResultBuilder>().AsImplementedInterfaces();
        }
    }
}
