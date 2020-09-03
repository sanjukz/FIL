using Autofac;
using FIL.Web.Core;
using FIL.Web.Admin.Providers;
using FIL.Web.Providers.Reporting;
using FIL.Messaging.Senders;
using FIL.MailChimp;

namespace FIL.Web.Admin.Modules
{
  public class AutofacModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<ConfirmationEmailSender>().As<IConfirmationEmailSender>();
      builder.RegisterType<TwilioTextMessageSender>().As<ITwilioTextMessageSender>().InstancePerLifetimeScope();
      builder.RegisterType<GupShupTextMessageSender>().As<IGupShupTextMessageSender>();
      builder.RegisterType<SiteIdProvider>().AsImplementedInterfaces();
      builder.RegisterType<AmazonS3FileProvider>().As<IAmazonS3FileProvider>();
      //builder.RegisterType<ReportColumnProvider>().As<IReportColumnProvider>();
      builder.RegisterType<AccountEmailSender>().As<IAccountEmailSender>();
      builder.RegisterType<UtcTimeProvider>().As<IUtcTimeProvider>();
      builder.RegisterType<LocalTimeProvider>().As<ILocalTimeProvider>();
      builder.RegisterType<MailChimpProvider>().As<IMailChimpProvider>();
    }
  }
}
