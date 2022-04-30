namespace TranslationSite.Infrastructure
{
    public class RequestValidationResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public static RequestValidationResult Success() => new RequestValidationResult { IsSuccess = true };

        public static RequestValidationResult Failed(string message) => new RequestValidationResult 
        { 
            IsSuccess = false,
            Message = message
        };
    }
}
