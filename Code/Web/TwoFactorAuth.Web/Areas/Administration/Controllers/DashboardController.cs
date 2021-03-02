using TwoFactorAuth.Services.Data.SettingsService;

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
            return View(new IndexViewModel
            {
                SettingsCount = _settingsService.GetCount(),
            });
        }
    }
}
