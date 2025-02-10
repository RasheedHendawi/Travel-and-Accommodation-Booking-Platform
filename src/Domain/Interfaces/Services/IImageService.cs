using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services
{
    public interface IImageService
    {
          Task<Image> StoreAsync(IFormFile image);
          Task DeleteAsync(Image image);
    }
}
