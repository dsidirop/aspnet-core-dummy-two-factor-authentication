using System.Threading.Tasks;

namespace TwoFactorAuth.Services.Auth.Contracts
{
    public interface IPasswordHintImageService
    {
        Task SpawnSecondStagePasswordHintImageAsync();
        string GetLoginSecondStepImageHintFilePath();
    }
}