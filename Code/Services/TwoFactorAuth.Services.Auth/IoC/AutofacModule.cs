// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantTypeArgumentsOfMethod

using TwoFactorAuth.Services.Auth.PasswordHintImage;

namespace TwoFactorAuth.Services.Auth.IoC
{
    using Autofac;

    using TwoFactorAuth.Services.Auth.Contracts;
    using TwoFactorAuth.Services.Auth.DummyAuth;

    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType(typeof(DummyTwoFactorAuthService))
                .As(typeof(IDummyTwoFactorAuthService))
                .InstancePerLifetimeScope();

            builder
                .RegisterType(typeof(PasswordHintImageService))
                .As(typeof(IPasswordHintImageService))
                .SingleInstance();
        }
    }
}