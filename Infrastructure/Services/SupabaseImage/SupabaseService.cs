using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Supabase;
using Domain.Entities;

namespace Infrastructure.Services.SupabaseImage
{
    public class SupabaseService : IImageService
    {
        private static readonly string[] AllowedImageFormats = { ".jpg", ".jpeg", ".png" };
        private readonly Client _supabaseClient;
        private readonly SupabaseConfig _supabaseConfig;

        public SupabaseService(IOptions<SupabaseConfig> supabaseConfig)
        {
            _supabaseConfig = supabaseConfig.Value;

            _supabaseClient = new Client(_supabaseConfig.Url, _supabaseConfig.Key);
            _supabaseClient.InitializeAsync().Wait();
        }

        public async Task<Image> StoreAsync(IFormFile image)
        {
            if (image == null || image.Length <= 0)
                throw new ArgumentNullException(nameof(image), "Image cannot be null or empty.");

            var imageFormat = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!IsAllowedImageFormat(imageFormat))
                throw new ArgumentOutOfRangeException(nameof(imageFormat), "Unsupported image format.");

            var imageModel = new Image { Format = imageFormat.TrimStart('.') };

            var destinationObjectName = $"{imageModel.Id}{imageFormat}";

            var storage = _supabaseClient.Storage.From(_supabaseConfig.Bucket);

            var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            await storage.Upload(fileBytes, destinationObjectName);

            imageModel.Path = GetPublicUrl(destinationObjectName);

            return imageModel;
        }

        public async Task DeleteAsync(Image image)
        {
            var storage = _supabaseClient.Storage.From(_supabaseConfig.Bucket);

            var fileName = $"{image.Id}.{image.Format}";

            await storage.Remove(fileName);
        }

        private static bool IsAllowedImageFormat(string imageFormat) =>
            AllowedImageFormats.Contains(imageFormat);

        private string GetPublicUrl(string fileName)
        {
            var storage = _supabaseClient.Storage.From(_supabaseConfig.Bucket);
            return storage.GetPublicUrl(fileName);
        }
    }
}
