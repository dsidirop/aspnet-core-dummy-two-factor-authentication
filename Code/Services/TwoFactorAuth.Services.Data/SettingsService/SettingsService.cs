using System.Collections.Generic;
using System.Linq;
using TwoFactorAuth.Data.Common.Repositories;
using TwoFactorAuth.Data.Models;
using TwoFactorAuth.Services.Mapping;

namespace TwoFactorAuth.Services.Data.SettingsService
{
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
