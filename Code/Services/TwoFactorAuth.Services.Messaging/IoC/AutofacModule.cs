// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace TwoFactorAuth.Services.Messaging.IoC
{
    using Autofac;
    using TwoFactorAuth.Services.Messaging.Contracts;

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NullMessageSender>().As<IAppEmailSender>().InstancePerDependency();
            //builder.RegisterType<SendGridEmailSender>().As<IAppEmailSender>().InstancePerDependency();
        }
    }
}
