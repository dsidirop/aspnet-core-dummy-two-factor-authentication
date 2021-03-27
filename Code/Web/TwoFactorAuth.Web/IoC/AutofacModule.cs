// ReSharper disable RedundantTypeArgumentsOfMethod

namespace TwoFactorAuth.Web.IoC
{
    using Autofac;
    using MediatR.Extensions.Autofac.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using TwoFactorAuth.Web.Infrastructure.Mediator.Commands.Auth.FirstStagePasswordValidation;
    using TwoFactorAuth.Web.StartupX;

    public class AutofacModule : Module
    {
        private readonly IConfiguration _appConfiguration;

        public AutofacModule(IConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IConfiguration>(_ => _appConfiguration).SingleInstance();

            builder.RegisterMediatR(typeof(FirstStagePasswordValidationCommand).Assembly); //TwoFactorAuth.Web.Infrastructure

            builder.ScanAllSolutionAssembliesInExecutingDirectory();
        }
    }
}
