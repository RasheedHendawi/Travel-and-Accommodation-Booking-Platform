using Application.DTOs.Users;
using Application.Services.Users;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Authentication;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Moq;


namespace Application.Tests.Services.Users
{
    public class UserServiceShould
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceShould()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtGeneratorMock = new Mock<IJwtGenerator>();
            _mapperMock = new Mock<IMapper>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _roleRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtGeneratorMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task ReturnTokenWhenCredentialsAreValid()
        {
            var email = "testing@gmail.com";
            var password = "123456";
            var user = new User { Id = new Guid(), Email = email };
            JwtToken expectedToken = new("mockedToken");

            _userRepositoryMock
                .Setup(repo => repo.AuthenticateAsync(email, password))
                .ReturnsAsync(user);

            _jwtGeneratorMock
                .Setup(jwt => jwt.GenerateToken(user))
                .Returns(expectedToken);

            _mapperMock
                .Setup(mapper => mapper.Map<LoginResponse>(expectedToken))
                .Returns(new LoginResponse { Token = expectedToken.Token });


            var result = await _userService.LoginAsync(email, password);

            Assert.NotNull(result);
            Assert.Equal(expectedToken.Token, result.Token);
        }

        [Fact]
        public async Task ThrowExceptionWhenCredentialsAreInvalid()
        {

            var email = "invalid@gmail.com";
            var password = "randomWrongPassword";

            _userRepositoryMock
                .Setup(repo => repo.AuthenticateAsync(email, password))
                .ReturnsAsync(null as User);

            await Assert.ThrowsAsync<Exception>(() => _userService.LoginAsync(email, password));
        }

        [Fact]
        public async Task RegisterUserWhenDataIsValid()
        {
            var registerRequest = new RegisterRequest
            {
                Email = "newuser@gmail.com",
                Password = "123456"
            };
            var roleGuied = new Guid();
            var userGuied = new Guid();
            var role = new Role { Id = roleGuied, Name = "Guest" };
            var user = new User { Id = userGuied, Email = registerRequest.Email, Roles = [] };

            _roleRepositoryMock
                .Setup(repo => repo.GetByNameAsync("Guest"))
                .ReturnsAsync(role);

            _userRepositoryMock
                .Setup(repo => repo.ExistsByEmailAsync(registerRequest.Email))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(mapper => mapper.Map<User>(registerRequest))
                .Returns(user);

            _userRepositoryMock
                .Setup(repo => repo.CreateAsync(user))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(uow => uow.SaveChangesAsync())
                .ReturnsAsync(1);


            await _userService.RegisterGuestAsync(registerRequest);

            _userRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ThrowExceptionWhenUserAlreadyExists()
        {
            var registerRequest = new RegisterRequest { Email = "existing@gmail.com" };

            _userRepositoryMock
                .Setup(repo => repo.ExistsByEmailAsync(registerRequest.Email))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _userService.RegisterGuestAsync(registerRequest));
        }
    }

}
