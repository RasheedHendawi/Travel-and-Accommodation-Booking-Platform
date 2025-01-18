using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
namespace Domain.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task DeleteAsync(Guid id, ImageType type);
        Task<Image> CreateAsync(IFormFile image, Guid id, ImageType type);
    }
}
