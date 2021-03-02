using System.Collections.Generic;

namespace TwoFactorAuth.Services.Data.SettingsService
{
    public interface ISettingsService
    {
        int GetCount();

        IEnumerable<T> GetAll<T>();
    }
}
