namespace TwoFactorAuth.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Mapping;

    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Models;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> _settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public int GetCount()
        {
            return _settingsRepository.AllAsNoTracking().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _settingsRepository.All().To<T>().ToList();
        }
    }
}
