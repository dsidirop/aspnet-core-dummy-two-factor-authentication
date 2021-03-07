﻿// ReSharper disable RedundantTypeArgumentsOfMethod

namespace TwoFactorAuth.Web.IoC
{
    using Autofac;

    using Microsoft.Extensions.Configuration;

    using TwoFactorAuth.Web.StartupX;

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

            builder.ScanAllSolutionAssembliesInExecutingDirectory();
        }
    }
}