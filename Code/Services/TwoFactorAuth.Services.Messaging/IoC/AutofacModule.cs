// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantTypeArgumentsOfMethod

using TwoFactorAuth.Services.Messaging.Contracts;

namespace TwoFactorAuth.Services.Messaging.IoC
{
    using Autofac;

    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NullMessageSender>().As<IAppEmailSender>().InstancePerDependency();
            //builder.RegisterType<SendGridEmailSender>().As<IAppEmailSender>().InstancePerDependency();
        }
    }
}