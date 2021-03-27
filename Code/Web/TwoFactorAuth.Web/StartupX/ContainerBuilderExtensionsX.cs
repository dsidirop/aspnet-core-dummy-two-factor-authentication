namespace TwoFactorAuth.Web.StartupX
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Autofac;

    static public class ContainerBuilderExtensionsX
    {
        static public ContainerBuilder ScanAllSolutionAssembliesInExecutingDirectory(
            this ContainerBuilder builder,
            string whitelistSearchPattern = @"(TwoFactorAuth[.].*[.]dll)$",
            string blacklistSearchPattern = @"(.*[.]Web[.]dll|.*[.]Contracts[.]dll)$"
        )
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            var whitelistRegex = new Regex(whitelistSearchPattern, RegexOptions.IgnoreCase);
            var blacklistRegex = new Regex(blacklistSearchPattern, RegexOptions.IgnoreCase);

            var dllPaths = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

            dllPaths = dllPaths
                .Select(x => new {fullpath = x, filename = Path.GetFileName(x)})
                .Where(pair => whitelistRegex.IsMatch(pair.filename))
                .Where(pair => !blacklistRegex.IsMatch(pair.filename))
                .Select(x => x.fullpath)
                .ToArray();

            foreach (var p in dllPaths)
            {
                var loadedAssembly = Assembly.LoadFrom(p);

                builder.RegisterAssemblyModules(loadedAssembly);
            }

            return builder;
        }
    }
}
