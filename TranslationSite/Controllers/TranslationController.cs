using Microsoft.AspNetCore.Mvc;
using WordParsing.Logic;
using TranslationSite.Dto;
using TranslationSite.Infrastructure;

namespace TranslationSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslationController : ControllerBase
    {
        private readonly ILogger<TranslationController> _logger;
        private readonly IWordParsingService _parsingService;
        private readonly ITranslationService _translationService;
        private readonly ITemporaryFileStorage _fileStorage;
        private readonly IRequestValidationHelper _validator;

        public TranslationController(
            ILogger<TranslationController> logger,
            IWordParsingService parsingService,
            ITranslationService translationService,
            ITemporaryFileStorage fileStorage,
            IRequestValidationHelper validator)
        {
            _logger = logger;
            _parsingService = parsingService;
            _translationService = translationService;
            _fileStorage = fileStorage;
            _validator = validator;
        }

        [Route("Languages", Name = "Returns list of supported languages")]
        [HttpGet]
        public async Task<IActionResult> GetLanguages()
        {
            var langs = await _translationService.GetLanguagesAsync().ConfigureAwait(false);

            return Ok(langs.Select(x => new LanguageDto
            {
                Name = x.Name,
                Code = x.Code
            }).OrderBy(x => x.Code));
        }

        [Route("Translate", Name = "Translates fragments of provided file")]
        [HttpPost]
        public async Task<IActionResult> TranslateAsync([FromForm] TranslateDto request)
        {
            var validationResult = await _validator.Validate(request);
            if (!validationResult.IsSuccess)
                return BadRequest(validationResult.Message);

            var filePath = string.IsNullOrWhiteSpace(request.FileUrl)
                    ? await _fileStorage.StoreFile(request.FormFile)
                    : await _fileStorage.StoreUrl(request.FileUrl);

            var nodes = _parsingService.GetNodes(filePath);
            var text = _parsingService.GetSingleText(nodes);

            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("There is nothing to translate");
            }
            else
            {
                var translations = await _translationService.TranslateAsync(request.From, request.To, new[] { text }).ConfigureAwait(false);
                return Ok(translations);
            }
        }
    }
}
