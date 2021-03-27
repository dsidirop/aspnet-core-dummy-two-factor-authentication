namespace TwoFactorAuth.Services.Auth.Contracts
{
    using System.Threading.Tasks;

    public interface IPasswordHintImageService
    {
        Task SpawnSecondStagePasswordHintImageAsync();
        string GetLoginSecondStepImageHintFilePath();
    }
}
