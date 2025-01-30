using Application.Contracts;
using Application.DTOs.Owners;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using System.Linq.Expressions;

namespace Application.Owners
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OwnerService(IOwnerRepository ownerRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<OwnersResponse>> GetOwnersAsync(OwnersGetRequest request)
        {
            var query = new Query<Owner>(
                GetSearchExpression(request.SearchTerm),
                request.SortOrder ?? SortMethod.Ascending,
                request.SortColumn,
                request.PageNumber,
                request.PageSize);

            var owners = await _ownerRepository.GetAsync(query);
            return _mapper.Map<PaginatedList<OwnersResponse>>(owners);
        }

        public async Task<OwnersResponse> GetOwnerByIdAsync(Guid id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id)
                         ?? throw new Exception("Owner not found.");

            return _mapper.Map<OwnersResponse>(owner);
        }

        public async Task<OwnersResponse> CreateOwnerAsync(OwnerCreationRequest request)
        {
            var ownerEntity = _mapper.Map<Owner>(request);
            var createdOwner = await _ownerRepository.CreateAsync(ownerEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OwnersResponse>(createdOwner);
        }

        public async Task UpdateOwnerAsync(Guid id, OwnerUpdateRequest request)
        {
            var ownerEntity = await _ownerRepository.GetByIdAsync(id)
                              ?? throw new Exception("Owner not found.");

            _mapper.Map(request, ownerEntity);

            await _ownerRepository.UpdateAsync(ownerEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        private static Expression<Func<Owner, bool>> GetSearchExpression(string? searchTerm)
        {
            return searchTerm is null
                ? _ => true
                : o => o.FirstName.Contains(searchTerm) || o.LastName.Contains(searchTerm);
        }
    }
}
