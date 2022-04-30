using TranslationSite.Dto;
using WordParsing.Logic;

namespace TranslationSite.Infrastructure
{
    public class RequestValidationHelper : IRequestValidationHelper
    {
        private readonly IWordParsingService _parsingService;
        private readonly ITranslationService _translationService;

        public RequestValidationHelper(
            IWordParsingService parsingService,
            ITranslationService translationService)
        {
            _parsingService = parsingService;
            _translationService = translationService;
        }

        public async Task<RequestValidationResult> Validate(TranslateDto request)
        {
            if (request == null)
                return RequestValidationResult.Failed("Invalid parameters");

            var fileResult = !string.IsNullOrWhiteSpace(request.FileUrl) ? ValidateFileUrl(request.FileUrl) : ValidateFile(request);
            if (fileResult != null)
            {
                return fileResult;
            }

            if (request.From == request.To && !string.IsNullOrWhiteSpace(request.To))
            {
                return RequestValidationResult.Failed("Select different languages");
            }

            var languages = await _translationService.GetLanguagesAsync().ConfigureAwait(false);
            if (languages.All(x => x.Code != request.From) || languages.All(x => x.Code != request.To))
            {
                return RequestValidationResult.Failed($"Unable to find language pair from: {request.From} to: {request.To}. Supported language codes are: {string.Join(',', languages.Select(x => x.Code))}");
            }

            return RequestValidationResult.Success();
        }

        private RequestValidationResult ValidateFile(TranslateDto request)
        {
            if (!_parsingService.ValidateFileName(request.FileName))
            {
                return RequestValidationResult.Failed("File name extension 'doc' or 'docx' is expected");
            }

            if (request.FormFile == null)
            {
                return RequestValidationResult.Failed("File data should be provided");
            }

            if (request.FormFile.Length > ITemporaryFileStorage.MaxAllowedFileSize)
            {
                return RequestValidationResult.Failed($"File size exceed {ITemporaryFileStorage.MaxAllowedFileSize}");
            }

            return RequestValidationResult.Success();
        }

        private RequestValidationResult ValidateFileUrl(string url)
        {
            if (!_parsingService.ValidateFileName(url))
            {
                return RequestValidationResult.Failed("File name extension 'doc' or 'docx' is expected");
            }

            return RequestValidationResult.Success();
        }
    }
}
