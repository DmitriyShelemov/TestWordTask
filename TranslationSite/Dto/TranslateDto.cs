namespace TranslationSite.Dto
{
    public class TranslateDto
    {
        public string To { get; set; }

        public string From { get; set; }

        public string FileUrl { get; set; }

        public string FileName { get; set; }

        public IFormFile FormFile { get; set; }
    }
}
