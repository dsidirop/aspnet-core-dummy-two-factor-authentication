namespace TwoFactorAuth.Web.Tests
{
    using System;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using Xunit;

    public class SeleniumTests : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        private readonly IWebDriver _browser;
        private readonly SeleniumServerFactory<Startup> _server;

        public SeleniumTests(SeleniumServerFactory<Startup> server)
        {
            _server = server;
            server.CreateClient();
            var opts = new ChromeOptions();
            opts.AddArguments("--headless");
            opts.AcceptInsecureCertificates = true;
            _browser = new ChromeDriver(opts);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact(Skip = "Example test. Disabled for CI.")]
        public void FooterOfThePageContainsPrivacyLink()
        {
            _browser.Navigate().GoToUrl(_server.RootUri);
            Assert.EndsWith(
                "/Home/Privacy",
                _browser.FindElements(By.CssSelector("footer a")).First().GetAttribute("href"));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _server?.Dispose();
                _browser?.Dispose();
            }
        }
    }
}
