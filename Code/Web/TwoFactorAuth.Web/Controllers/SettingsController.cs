﻿namespace TwoFactorAuth.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Models;
    using TwoFactorAuth.Services.Data.Contracts;
    using TwoFactorAuth.Web.Infrastructure.Controllers;
    using TwoFactorAuth.Web.ViewModels.Settings;

    public class SettingsController : PlatformBaseController
    {
        private readonly IDeletableEntityRepository<Setting> _repository;
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService, IDeletableEntityRepository<Setting> repository)
        {
            _repository = repository;
            _settingsService = settingsService;
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
