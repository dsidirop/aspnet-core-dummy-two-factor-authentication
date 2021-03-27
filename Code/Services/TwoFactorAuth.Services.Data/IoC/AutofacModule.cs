// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace TwoFactorAuth.Services.Data.IoC
{
    using Autofac;
    using TwoFactorAuth.Services.Data.Contracts;
    using TwoFactorAuth.Services.Data.Settings;

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(SettingsService)).As(typeof(ISettingsService)).InstancePerLifetimeScope();
        }
    }
}
