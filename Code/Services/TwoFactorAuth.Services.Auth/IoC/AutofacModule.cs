// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace TwoFactorAuth.Services.Auth.IoC
{
    using Autofac;
    using TwoFactorAuth.Services.Auth.Contracts;
    using TwoFactorAuth.Services.Auth.DummyAuth;
    using TwoFactorAuth.Services.Auth.PasswordHintImage;

    public class AutofacModule : Module
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
