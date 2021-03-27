// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace TwoFactorAuth.Data.IoC
{
    using Autofac;
    using TwoFactorAuth.Data.Common;
    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Repositories;

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(DbQueryRunner)).As(typeof(IDbQueryRunner)).InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfDeletableEntityRepository<>)).As(typeof(IDeletableEntityRepository<>)).InstancePerLifetimeScope();
        }
    }
}
