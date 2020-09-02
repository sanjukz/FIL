using Autofac;
using FIL.Messaging.Senders;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Module = Autofac.Module;

namespace FIL.Messaging.Modules
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
            builder.RegisterModule(new Configuration.Modules.AutofacModule(_configurationRoot));
            builder.RegisterModule(new Logging.Modules.AutofacModule(_configurationRoot));

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.Contains("Messenger"))
                .AsImplementedInterfaces();

            builder.RegisterType<TwilioTextMessageSender>().As<ITwilioTextMessageSender>();
            builder.RegisterType<KapTextMessageSender>().As<IKapTextMessageSender>();
            builder.RegisterType<GupShupTextMessageSender>().As<IGupShupTextMessageSender>();
            builder.RegisterType<SmtpEmailSender>().As<ISmtpEmailSender>();
            builder.RegisterType<SmtpBulkEmailSender>().As<ISmtpBulkEmailSender>();
            builder.RegisterType<ConfirmationEmailSender>().As<IConfirmationEmailSender>();
        }
    }
}