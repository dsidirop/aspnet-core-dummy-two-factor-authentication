using Microsoft.Extensions.Configuration;
using TwoFactorAuth.Common.Contracts.Configuration;

// ReSharper disable RedundantTypeArgumentsOfMethod

namespace TwoFactorAuth.Web.StartupX
{
    using Autofac;

    using TwoFactorAuth.Data;
    using TwoFactorAuth.Data.Common;
    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Repositories;
    using TwoFactorAuth.Services.Auth.Contracts;
    using TwoFactorAuth.Services.Auth.DummyAuth;
    using TwoFactorAuth.Services.Contracts;
    using TwoFactorAuth.Services.Crypto;
    using TwoFactorAuth.Services.Data.Contracts;
    using TwoFactorAuth.Services.Data.Settings;
    using TwoFactorAuth.Services.Messaging;
    using TwoFactorAuth.Services.Messaging.Contracts;

    public class AutofacModule : Autofac.Module
    {
        private readonly IConfiguration _appConfiguration;

        public AutofacModule(IConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IConfiguration>(_ => _appConfiguration).SingleInstance();

            builder.RegisterType(typeof(DbQueryRunner)).As(typeof(IDbQueryRunner)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(SettingsService)).As(typeof(ISettingsService)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(DummyTwoFactorAuthService)).As(typeof(IDummyTwoFactorAuthService)).InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfDeletableEntityRepository<>)).As(typeof(IDeletableEntityRepository<>)).InstancePerLifetimeScope();

            builder.RegisterType<AppCryptoService>().As<IAppCryptoService>().InstancePerDependency();
            builder.RegisterType<NullMessageSender>().As<IAppEmailSender>().InstancePerDependency();

            builder.ScanAllSolutionAssembliesInExecutingDirectory();
        }
    }
}