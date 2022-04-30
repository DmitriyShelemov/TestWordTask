using CommandLine;

namespace WordParsing
{
    internal class CommandLineOptions
    {
        [Option('p', "path", Required = true, HelpText = "Full path to Word document")]
        public string FilePath { get; set; }

        [Option('f', "languagefrom", Required = true, HelpText = "Language from")]
        public string LanguageFrom { get; set; }

        [Option('t', "languageto", Required = true, HelpText = "Language to")]
        public string LanguageTo { get; set; }
    }
}
