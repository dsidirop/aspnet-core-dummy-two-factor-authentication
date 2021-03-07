namespace TwoFactorAuth.Services.Data.Settings
{
    using System.Collections.Generic;
    using System.Linq;

    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Models;
    using TwoFactorAuth.Services.Data.Contracts;
    using TwoFactorAuth.Services.Mapping;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> _settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public int GetCount()
        {
            return _settingsRepository.AllCount();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _settingsRepository
                .All()
                .To<T>() //magic mapping through automapper   Setting => T
                .ToList();
        }
    }
}
