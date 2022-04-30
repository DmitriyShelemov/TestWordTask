using TranslationSite.Dto;

namespace TranslationSite.Infrastructure
{
    public interface IRequestValidationHelper
    {
        Task<RequestValidationResult> Validate(TranslateDto request);
    }
}