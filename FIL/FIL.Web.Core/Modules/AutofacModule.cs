using Autofac;
using FIL.Web.Core.ErrorMessageProviders;
using FIL.Web.Core.Helpers;
using FIL.Web.Core.Providers;
using FIL.Web.Core.Providers.Sitemap;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SimpleMvcSitemap;
using SimpleMvcSitemap.Routing;
using FIL.Web.Core.UrlsProvider;

namespace FIL.Web.Core.Modules
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration _configurationRoot;

        public AutofacModule(IConfiguration configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => _configurationRoot);
            builder.RegisterModule(new Logging.Modules.AutofacModule(_configurationRoot));
            builder.RegisterModule(new Configuration.Modules.AutofacModule(_configurationRoot));
            builder.RegisterModule(new Foundation.Modules.AutofacModule(_configurationRoot));

            builder.RegisterType<AuthenticationHelper>().AsImplementedInterfaces();
            builder.RegisterType<SessionProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<PasswordHasher<string>>().As<IPasswordHasher<string>>();
            builder.RegisterType<PaymentErrorMessageProvider>().As<IPaymentErrorMessageProvider>();
            builder.RegisterType<BaseUrlProvider>().As<IBaseUrlProvider>().InstancePerLifetimeScope();
            builder.RegisterType<SiteUrlsProvider>().As<ISiteUrlsProvider>().InstancePerLifetimeScope();
            builder.Register(c => new SitemapProvider(c.Resolve<IBaseUrlProvider>())).As<ISitemapProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ClientIpProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DynamicSourceProvider>().As<IDynamicSourceProvider>().InstancePerLifetimeScope();
            builder.RegisterType<OTPProvider>().As<IOTPProvider>().InstancePerLifetimeScope();
        }
    }
}
