using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using WordParsing.Logic;

namespace WordParsing
{
    class Program
    {
        private const string YaKey = "AQAAAAAAedgmAATuwfTP2xUfAUSOusEBfOQf9dc";
        private const string YaFolderId = "b1g19vmv970d559ph01q";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    IWordParsingService parsingService = new WordParsingService(new AsposeWrapper());

                    var invalid = false;

                    if (!parsingService.ValidateFile(o.FilePath))
                    {
                        Console.WriteLine($"File path is incorrect '{o.FilePath}'");
                        invalid = true;
                    }

                    ITranslationService translationService = new TranslationService(YaKey, YaFolderId);
                    var languages = translationService.GetLanguages();
                    if (languages.All(x => x.Code != o.LanguageFrom) || languages.All(x => x.Code != o.LanguageTo))
                    {
                        Console.WriteLine($"Unable to find language pair from: {o.LanguageFrom} to: {o.LanguageTo}");
                        Console.WriteLine("Supported languages are:");
                        foreach (var line in MakeMultiline(new[] { string.Join(',', languages.Select(x => x.Code)) }))
                        {
                            Console.WriteLine(line);
                        }
                        invalid = true;
                    }

                    if (invalid)
                    {
                        Console.WriteLine("Please press any can to close");
                    }
                    else
                    {
                        try
                        {
                            var nodes = parsingService.GetNodes(o.FilePath);
                            var texts = parsingService.GetTexts(nodes);

                            if (!texts.Any())
                            {
                                Console.WriteLine("There is nothing to translate");
                            }
                            else
                            {
                                var translations = translationService.Translate(o.LanguageFrom, o.LanguageTo, texts);
                                Console.WriteLine("Translations are:");
                                foreach (var line in MakeMultiline(translations))
                                {
                                    Console.WriteLine(line);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                });

            Console.ReadKey();
            return;
        }

        private static IEnumerable<string> MakeMultiline(IEnumerable<string> texts)
        {
            const int chunkSize = 100;
            return texts.SelectMany(str => SplitText(str, chunkSize));
        }

        private static IEnumerable<string> SplitText(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, Math.Min(chunkSize, str.Length - i));
        }
    }
}
