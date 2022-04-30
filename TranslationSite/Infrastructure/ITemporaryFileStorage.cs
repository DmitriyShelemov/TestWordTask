
namespace TranslationSite.Infrastructure
{
    public interface ITemporaryFileStorage
    {
        public const int MaxAllowedFileSize = 3 * 1024 * 1024;

        Task<string> StoreFile(IFormFile file);
        Task<string> StoreUrl(string fileUrl);
    }
}