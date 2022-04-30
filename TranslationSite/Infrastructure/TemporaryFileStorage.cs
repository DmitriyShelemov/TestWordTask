namespace TranslationSite.Infrastructure
{
    public class TemporaryFileStorage : ITemporaryFileStorage
    {
        public async Task<string> StoreFile(IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            await using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
                return filePath;
            }
        }

        public async Task<string> StoreUrl(string fileUrl)
        {
            var filePath = Path.GetTempFileName();
            using (var client = new HttpClient())
            {
                using (var inputStream = await client.GetStreamAsync(fileUrl))
                {
                    await using (var stream = File.Create(filePath))
                    {
                        await inputStream.CopyToAsync(stream);

                        // TODO: think how to read headers prior loading stream
                        if (stream.Length > ITemporaryFileStorage.MaxAllowedFileSize)
                        {
                            throw new InvalidOperationException($"File size exceed {ITemporaryFileStorage.MaxAllowedFileSize}");
                        }
                        return filePath;
                    }
                }
            }
        }
    }
}
