// ReSharper disable ConvertToConstant.Local
namespace TwoFactorAuth.Web.StartupX
{
    using System;
    using Autofac;
    using Autofac.Diagnostics;

    static internal class AddAutofaqX
    {
        static private readonly bool EnableDiagnostics = Environment.GetEnvironmentVariable("ASPNETCORE_ENABLE_DI_DIAGNOSTICS")?.ToLowerInvariant() == "true";

        static public ContainerBuilder AddAutofaqDiagnostics(this ContainerBuilder autofaqBuilder)
        {
            if (!EnableDiagnostics)
            {
                return autofaqBuilder;
            }

            var tracer = new DefaultDiagnosticTracer(); //0

            tracer.OperationCompleted += (_, args) => { Console.WriteLine(args.TraceContent); };

            autofaqBuilder.RegisterBuildCallback(lifetimeScope =>
            {
                var container = (IContainer) lifetimeScope;

                container.SubscribeToDiagnostics(tracer);
            });

            return autofaqBuilder;

            //0 Add any Autofac modules or registrations  This is called AFTER ConfigureServices so things we
            //  register here OVERRIDE things registered in ConfigureServices
            //
            //  We must have the call to UseServiceProviderFactory(new AutofacServiceProviderFactory())
            //  when building the host or this wont be called
            //
            //  Diagnostics get printed via a build callback   Diagnostics arent free so we shouldnt just do this
            //  by default
            //
            //  Note  since we are diagnosing the container we cant ALSO resolve the logger to which the diagnostics
            //  get written so writing directly to the log destination is the way to go
        }
    }
}
