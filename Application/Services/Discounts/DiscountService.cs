

using Application.Contracts;
using Application.DTOs.Discounts;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using System.Threading;

namespace Application.Services.Discounts
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DiscountService(
            IRoomClassRepository roomClassRepository,
            IDiscountRepository discountRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _roomClassRepository = roomClassRepository;
            _discountRepository = discountRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<DiscountResponse>> GetAmenitiesAsync(Guid roomClassId, DiscountsGetRequest request)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }

            var query = new Query<Discount>(
              d => d.RoomClassId == roomClassId,
              request.SortOrder ?? SortMethod.Ascending,
              request.SortColumn,
              request.PageNumber,
              request.PageSize);
            var owners = await _discountRepository.GetAsync(query);
            return _mapper.Map<PaginatedList<DiscountResponse>>(owners);
        }

        public async Task<DiscountResponse> GetDiscountByIdAsync(Guid roomClassId, Guid discountId)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }

            var discount = await _discountRepository.GetByIdAsync(roomClassId, discountId);
            if (discount == null)
            {
                throw new Exception("Discount Not Found In RoomClass");
            }

            return _mapper.Map<DiscountResponse>(discount);
        }

        public async Task<DiscountResponse> CreateDiscountAsync(Guid roomClassId, DiscountCreationRequest request)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }

            if ( await _discountRepository.ExistsAsync(d => request.EndDate >= d.StartDate && request.StartDate <= d.EndDate))
            {
                throw new Exception("Discount Intervals Conflict");
            }

            var discount = _mapper.Map<Discount>(request);
            discount.RoomClassId = roomClassId;
            discount.CreatedAt = DateTime.UtcNow;

            var createdDiscount = await _discountRepository.CreateAsync(discount);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DiscountResponse>(createdDiscount);
        }

        public async Task DeleteDiscountAsync(Guid roomClassId, Guid discountId)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }

            if (!await _discountRepository.ExistsAsync(d => d.Id == discountId && d.RoomClassId == roomClassId))
            {
                throw new Exception("Discount Not Found In Room Class");
            }

            await _discountRepository.DeleteAsync(discountId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
