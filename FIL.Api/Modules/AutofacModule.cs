using Autofac;
using HubSpot.NET.Core;
using HubSpot.NET.Core.Interfaces;
using FIL.Api.Core.Utilities;
using FIL.Api.Integrations;
using FIL.Api.Integrations.ASI;
using FIL.Api.Integrations.InfiniteAnalytics;
using FIL.Api.Integrations.InfiniteAnalytics.Recommendation;
using FIL.Api.Integrations.POne;
using FIL.Api.Integrations.ValueRetail;
using FIL.Api.Integrations.Zoom;
using FIL.Api.PaymentChargers;
using FIL.Api.Providers;
using FIL.Api.Providers.Algolia;
using FIL.Api.Providers.ASI;
using FIL.Api.Providers.EventManagement;
using FIL.Api.Providers.Export;
using FIL.Api.Providers.Transaction;
using FIL.Api.Providers.Zoom;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace FIL.Api.Modules
{
    public class AutofacModule : Autofac.Module
    {
        private readonly IConfiguration _configurationRoot;

        public AutofacModule(IConfiguration configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CurrencyConverter.Currency>().As<CurrencyConverter.ICurrencyConverter>();
            builder.RegisterType<SiteExtensions.GeoCurrency>().As<SiteExtensions.IGeoCurrency>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            builder.RegisterModule(new Logging.Modules.AutofacModule(_configurationRoot));
            builder.RegisterModule(new Configuration.Modules.AutofacModule(_configurationRoot));

            builder.Register(c => _configurationRoot);
            builder.Register(c => new HubSpotApi(c.Resolve<ISettings>().GetConfigSetting<string>(SettingKeys.Integration.HubSpot.ApiKey))).As<IHubSpotApi>();
            builder.RegisterType<EventLeanPageProvider>().As<IEventLearnPageProvider>().InstancePerLifetimeScope();
            builder.RegisterType<OrderConfirmationProvider>().As<IOrderConfirmationProvider>().InstancePerLifetimeScope();
            builder.RegisterType<CalendarProvider>().As<ICalendarProvider>().InstancePerLifetimeScope();
            builder.RegisterType<FeelExportIAProvider>().As<IFeelExportIAProvider>().InstancePerLifetimeScope();
            builder.RegisterType<DataSettings>().As<IDataSettings>().InstancePerLifetimeScope();
            builder.RegisterType<PasswordHasher<string>>().As<IPasswordHasher<string>>();
            builder.RegisterType<HdfcChargerResolver>().As<IHdfcChargerResolver>();
            builder.RegisterType<InitSession>().As<IInitSession>();
            builder.RegisterType<TransactionIdProvider>().As<ITransactionIdProvider>();
            builder.RegisterType<GetRecommendation>().As<IGetRecommendation>();
            builder.RegisterType<IpApi>().As<IIpApi>();
            builder.RegisterType<ValueRetailAuth>().As<IValueRetailAuth>();
            builder.RegisterType<ValueRetailAPI>().As<IValueRetailAPI>();
            builder.RegisterType<ShoppingCart>().As<IShoppingCart>();
            builder.RegisterType<ASIBookingProvider>().As<IASIBookingProvider>();
            builder.RegisterType<ASIApi>().As<IASIApi>();
            builder.RegisterType<SaveGuestUserProvider>().As<ISaveGuestUserProvider>();
            builder.RegisterType<SaveIPProvider>().As<ISaveIPProvider>();
            builder.RegisterType<SaveTransactionProvider>().As<ISaveTransactionProvider>();
            builder.RegisterType<SeatBlockingProvider>().As<ISeatBlockingProvider>();
            builder.RegisterType<TicketLimitProvider>().As<ITicketLimitProvider>();
            builder.RegisterType<UserProvider>().As<IUserProvider>();
            builder.RegisterType<Booking>().As<IBooking>();
            builder.RegisterType<GoogleMapApi>().As<IGoogleMapApi>();
            builder.RegisterType<LanguageTranslationApi>().As<ILanguageTranslationApi>();
            builder.RegisterType<CountryAlphaCode>().As<ICountryAlphaCode>();
            builder.RegisterType<SubCategoryProvider>().As<ISubCategoryProvider>();
            builder.RegisterType<PlaceProvider>().As<IPlaceProvider>();
            builder.RegisterType<DynamicSectionProvider>().As<IDynamicSectionProvider>();
            builder.RegisterType<ToEnglishTranslator>().As<IToEnglishTranslator>();
            builder.RegisterType<AlgoliaClientProvider>().As<IAlgoliaClientProvider>();
            builder.RegisterType<AlgoliaAddEventProvider>().As<IAlgoliaAddEventProvider>();
            builder.RegisterType<BookSeatTicketProvider>().As<IBookSeatTicketProvider>();
            builder.RegisterType<POneApi>().As<IPOneApi>();
            builder.RegisterType<POneBooking>().As<IPOneBooking>();
            builder.RegisterType<GenerateStripeConnectProvider>().As<IGenerateStripeConnectProvider>();
            builder.RegisterType<ZoomAPI>().As<IZoomAPI>();
            builder.RegisterType<ZoomMeetingProvider>().As<IZoomMeetingProvider>();
            builder.RegisterType<EventStripeConnectAccountProvider>().As<IEventStripeConnectAccountProvider>();
            builder.RegisterType<LocalTimeZoneConvertProvider>().As<ILocalTimeZoneConvertProvider>();
            builder.RegisterType<EventCurrencyProvider>().As<IEventCurrencyProvider>();
            builder.RegisterType<StepProvider>().As<IStepProvider>();
            builder.RegisterType<TransactionStatusUpdater>().As<ITransactionStatusUpdater>();
            builder.RegisterType<DiscountProvider>().As<IDiscountProvider>();
            builder.RegisterType<CommonUtilityProvider>().As<ICommonUtilityProvider>();
            builder.RegisterType<DeleteScheduleDetailProvider>().As<IDeleteScheduleDetailProvider>();
            builder.RegisterType<SaveScheduleDetailProvider>().As<ISaveScheduleDetailProvider>();
            builder.RegisterType<UpdateScheduleDetailProvider>().As<IUpdateScheduleDetailProvider>();
            builder.RegisterType<GetScheduleDetailProvider>().As<IGetScheduleDetailProvider>();
            builder.RegisterType<UtcTimeProvider>().As<IUtcTimeProvider>();
            builder.RegisterType<ReferralProvider>().As<IReferralProvider>();
            builder.RegisterType<SaveTransactionScheduleDetailProvider>().As<ISaveTransactionScheduleDetailProvider>();
            RegisterGroupAsImplementedInterface(builder, "Charger");
            RegisterGroupAsImplementedInterface(builder, "Repository");
            RegisterGroupAsImplementedInterface(builder, "QueryHandler");
            RegisterGroupAsImplementedInterface(builder, "CommandHandler");
            RegisterGroupAsImplementedInterface(builder, "Action");
            RegisterGroupAsImplementedInterface(builder, "Builder");
            RegisterGroupAsImplementedInterface(builder, "EventHandler");
            RegisterGroup(builder, "Mapper");

            RegisterMediatr(builder);
        }

        private void RegisterGroupAsImplementedInterface(ContainerBuilder builder, string sharedSuffix)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith(sharedSuffix))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        private void RegisterGroup(ContainerBuilder builder, string sharedSuffix)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith(sharedSuffix));
        }

        private void RegisterMediatr(ContainerBuilder builder)
        {
            // the Mediator class requires both of the following factories
            builder.Register<SingleInstanceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t =>
                {
                    object o;
                    return c.TryResolve(t, out o) ? o : null;
                };
            }).InstancePerLifetimeScope();

            builder.Register<MultiInstanceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            }).InstancePerLifetimeScope();

            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
        }
    }
}