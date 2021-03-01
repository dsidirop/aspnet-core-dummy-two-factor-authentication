namespace TwoFactorAuth.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using TwoFactorAuth.Services.Data;
    using TwoFactorAuth.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService _settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = _settingsService.GetCount(), };
            return View(viewModel);
        }
    }
}
