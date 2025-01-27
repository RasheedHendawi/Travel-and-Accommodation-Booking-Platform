using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Interfaces.Authentication;
using AutoMapper;
using Domain.Entities;
using Application.Contracts;
using Application.Users.Models;


namespace Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IJwtGenerator jwtGenerator,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var user = await _userRepository.AuthenticateAsync(email, password)
                       ?? throw new Exception("Creadtionals not Valid");

            var token = _jwtGenerator.GenerateToken(user);

            return _mapper.Map<LoginResult>(token);
        }

        public async Task RegisterGuestAsync(RegisterHandler registerRequest)
        {
            var role = await _roleRepository.GetByNameAsync(registerRequest.Role)
                       ?? throw new Exception("Invalid Role");

            if (await _userRepository.ExistsByEmailAsync(registerRequest.Email))
            {
                throw new Exception("User With Email Already Exists");
            }

            var user = _mapper.Map<User>(registerRequest);
            user.Roles.Add(role);

            await _userRepository.CreateAsync(user);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
