using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Persistence.ContextDb;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ImageRepository(HotelBookingPlatformDbContext context, IImageService imageService) : IImageRepository
    {
        public async Task<Image> CreateAsync(IFormFile image, Guid id, ImageType type)
        {
            var imageModel = await imageService.StoreAsync(image);
            var returnedImage = new Image
            {
                EntityId = id,
                Format = imageModel.Format,
                Path = imageModel.Path,
                Type = type
            };
            var CreatedImage = await context.Images.AddAsync(returnedImage);
            return CreatedImage.Entity;
        }

        public async Task DeleteAsync(Guid id, ImageType type)
        {
            var image = await context.Images.Where(i => i.EntityId == id && i.Type ==type)
                .ToListAsync();
            foreach (var img in image)
            {
                context.Images.Remove(img);
            }
        }
    }
}
