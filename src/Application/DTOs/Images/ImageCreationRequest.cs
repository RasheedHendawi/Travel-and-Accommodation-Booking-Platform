using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Images
{
    public class ImageCreationRequest
    {
        public IFormFile Image { get; init; }
    }
}
