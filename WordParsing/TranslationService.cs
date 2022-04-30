using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Cloud;
using Yandex.Cloud.Ai.Translate.V2;
using Yandex.Cloud.Credentials;
using static Yandex.Cloud.Ai.Translate.V2.TranslationService;

namespace WordParsing.Logic
{
    public class TranslationService : ITranslationService
    {
        private readonly Lazy<TranslationServiceClient> _client;
        private string _folderId;

        public TranslationService(string key, string folderId)
        {
            _client = new(() => new Sdk(new OAuthCredentialsProvider(key)).Services.Ai.Translate.TranslationService);
            _folderId = folderId;
        }

        public IList<string> Translate(string languageFrom, string languageTo, IEnumerable<string> texts)
        {
            if (string.IsNullOrEmpty(languageFrom))
            {
                throw new ArgumentNullException(nameof(languageFrom));
            }
            if (string.IsNullOrEmpty(languageTo))
            {
                throw new ArgumentNullException(nameof(languageTo));
            }
            if (texts == null)
            {
                throw new ArgumentNullException(nameof(texts));
            }

            if (!texts.Any())
                return Array.Empty<string>();

            var request = BuildRequest(languageFrom, languageTo, texts);

            return _client
                .Value
                .Translate(request)?
                .Translations?
                .Select(x => x.Text)
                .ToArray();
        }

        public async Task<IList<string>> TranslateAsync(string languageFrom, string languageTo, IEnumerable<string> texts)
        {
            if (string.IsNullOrEmpty(languageFrom))
            {
                throw new ArgumentNullException(nameof(languageFrom));
            }
            if (string.IsNullOrEmpty(languageTo))
            {
                throw new ArgumentNullException(nameof(languageTo));
            }
            if (texts == null)
            {
                throw new ArgumentNullException(nameof(texts));
            }

            if (!texts.Any())
                return Array.Empty<string>();

            var request = BuildRequest(languageFrom, languageTo, texts);

            var response = await _client
                .Value
                .TranslateAsync(request);

            return response?
                .Translations?
                .Select(x => x.Text)
                .ToArray();
        }

        public IReadOnlySet<Language> GetLanguages()
        {
            var response = _client
                .Value
                .ListLanguages(new ListLanguagesRequest { FolderId = _folderId }, new CallOptions().WithHeaders(Metadata.Empty));

            return response?
                    .Languages?
                    .ToHashSet();
        }

        public async Task<IReadOnlySet<Language>> GetLanguagesAsync()
        {
            var response = await _client
                .Value
                .ListLanguagesAsync(new ListLanguagesRequest { FolderId = _folderId }, new CallOptions().WithHeaders(Metadata.Empty));

            return response?
                    .Languages?
                    .ToHashSet();
        }

        private TranslateRequest BuildRequest(string languageFrom, string languageTo, IEnumerable<string> texts)
        {
            var request = new TranslateRequest
            {
                FolderId = _folderId,
                Format = TranslateRequest.Types.Format.PlainText,
                SourceLanguageCode = languageFrom,
                TargetLanguageCode = languageTo
            };

            request.Texts.AddRange(texts);
            return request;
        }
    }
}
