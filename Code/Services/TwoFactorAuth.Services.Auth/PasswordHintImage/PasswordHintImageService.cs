namespace TwoFactorAuth.Services.Auth.PasswordHintImage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using TwoFactorAuth.Common;
    using TwoFactorAuth.Common.Contracts;
    using TwoFactorAuth.Common.Contracts.Configuration;
    using TwoFactorAuth.Services.Auth.Contracts;

    public class PasswordHintImageService : IPasswordHintImageService
    {
        static private readonly string[] LoginSecondStepImageHintBaseFilePathComponents = GlobalConstants
            .LoginSecondStepBaseImagePasswordHintFilePath
            .Split(
                new[] {'/'},
                StringSplitOptions.RemoveEmptyEntries
            );

        static private readonly string[] LoginSecondStepImageHintEventualFilePathComponents = GlobalConstants
            .LoginSecondStepEventualImagePasswordHintFilePath
            .Split(
                new[] {'/'},
                StringSplitOptions.RemoveEmptyEntries
            );

        private readonly AppDummyAuthSpecs _appDummyAuthSpecs;
        private readonly IHostEnvironment _hostEnvironment;

        public PasswordHintImageService(
            IHostEnvironment hostEnvironment,
            IOptionsMonitor<AppDummyAuthSpecs> dummyAuthSpecsOptionsMonitor
        )
        {
            _hostEnvironment = hostEnvironment;
            _appDummyAuthSpecs = dummyAuthSpecsOptionsMonitor.CurrentValue;
        }

        private string GetResourceFilePath(IEnumerable<string> components)
        {
            return Path.Combine(
                _hostEnvironment
                    .ContentRootPath
                    .Enumify()
                    .Concat(components)
                    .ToArray()
            );
        }

        #region signin methods

        public async Task SpawnSecondStagePasswordHintImageAsync()
        {
            var baseImagePath = GetResourceFilePath(LoginSecondStepImageHintBaseFilePathComponents);
            var eventualImagePath = GetResourceFilePath(LoginSecondStepImageHintEventualFilePathComponents);

            File.Copy(baseImagePath, eventualImagePath, true); //order

            await using var output = new StreamWriter(eventualImagePath, true); //order

            await output.WriteLineAsync(
                @$"
/---------------------------------------/
You found it. The password is: {_appDummyAuthSpecs.DummyUsers.Second.Password}
/---------------------------------------/
"
            );
        }

        public string GetLoginSecondStepImageHintFilePath()
        {
            return GetResourceFilePath(LoginSecondStepImageHintEventualFilePathComponents);
        }

        #endregion
    }
}
