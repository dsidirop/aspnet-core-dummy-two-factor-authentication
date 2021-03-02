using TwoFactorAuth.Services.Data.SettingsService;

namespace TwoFactorAuth.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Data.Models;

    using Microsoft.AspNetCore.Mvc;

    using TwoFactorAuth.Data.Common.Repositories;

    using ViewModels.Settings;

    public class SettingsController : BaseController
    {
        private readonly ISettingsService _settingsService;

        private readonly IDeletableEntityRepository<Setting> _repository;

        public SettingsController(ISettingsService settingsService, IDeletableEntityRepository<Setting> repository)
        {
            _settingsService = settingsService;
            _repository = repository;
        }

        public IActionResult Index()
        {
            var settings = _settingsService.GetAll<SettingViewModel>();
            var model = new SettingsListViewModel { Settings = settings };
            return View(model);
        }

        public async Task<IActionResult> InsertSetting()
        {
            var random = new Random();
            var setting = new Setting { Name = $"Name_{random.Next()}", Value = $"Value_{random.Next()}" };

            await _repository.AddAsync(setting);
            await _repository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
