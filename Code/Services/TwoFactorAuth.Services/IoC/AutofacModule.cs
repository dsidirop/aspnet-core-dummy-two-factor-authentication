// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace TwoFactorAuth.Services.IoC
{
    using Autofac;
    using TwoFactorAuth.Services.Contracts;
    using TwoFactorAuth.Services.Crypto;

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppCryptoService>().As<IAppCryptoService>().InstancePerDependency();
        }
    }
}
