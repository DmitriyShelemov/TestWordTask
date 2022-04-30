using System.Collections.Generic;
using System.Threading.Tasks;
using Yandex.Cloud.Ai.Translate.V2;

namespace WordParsing.Logic
{
    public interface ITranslationService
    {
        IReadOnlySet<Language> GetLanguages();

        Task<IReadOnlySet<Language>> GetLanguagesAsync();

        IList<string> Translate(string languageFrom, string languageTo, IEnumerable<string> texts);

        Task<IList<string>> TranslateAsync(string languageFrom, string languageTo, IEnumerable<string> texts);
    }
}