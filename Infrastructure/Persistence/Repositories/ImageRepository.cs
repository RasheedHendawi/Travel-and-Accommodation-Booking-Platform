using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence.ContextDb;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ImageRepository(HotelBookingPlatformDbContext context) : IImageRepository
    {
        public async Task<Image> CreateAsync(IFormFile image, Guid id, ImageType type)
        {
            //TODO: Implement ImageRepository.CreateAsync
            throw new System.NotImplementedException();
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
