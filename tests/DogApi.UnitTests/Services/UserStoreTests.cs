using DogApi.Endpoints.Authentication.Models;
using DogApi.Endpoints.Authentication.Services;
using DogApi.Endpoints.Authentication.Utils;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System.Text.Json;

namespace DogApi.UnitTests.Services
{
    public class UserStoreTests
    {
        private readonly UserStore _userStore;
        private readonly Mock<IWebHostEnvironment> _mockEnv;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly List<User> _testUsers;

        public UserStoreTests()
        {
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _testUsers = new List<User>
            {
                new User { Username = "Kyle", PasswordHash = "HtFvu0lm8awI5ImnXpHAAK5zuLzvi7Nlz7qj2Wwmo94=", Salt = "oGmiYPy+0UmqoeuBTF38Mg==", Role = "Admin" },
            };

            var jsonData = JsonSerializer.Serialize(_testUsers);
            var tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, jsonData);

            _mockEnv.Setup(env => env.ContentRootPath).Returns(Path.GetDirectoryName(tempFilePath));

            // Initialize _userStore
            _userStore = new UserStore(_mockEnv.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public void ValidateUser_WithValidCredentials_ReturnsUser()
        {
            //var result = _mockPasswordHasher.Setup(ph => ph.VerifyPassword("adminpassword", "HtFvu0lm8awI5ImnXpHAAK5zuLzvi7Nlz7qj2Wwmo94=", "oGmiYPy+0UmqoeuBTF38Mg==")).Returns(true);

            // Act
            var user = _userStore.ValidateUser("Kyle", "adminpassword");

            // Assert
            Assert.NotNull(user);
            Assert.Equal("Kyle", user.Username);
        }

        [Fact]
        public void ValidateUser_ShouldReturnNull_WhenUsernameIsInvalid()
        {
            // Act
            var user = _userStore.ValidateUser("invaliduser", "password");

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void ValidateUser_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(ph => ph.VerifyPassword("wrongpassword", "hashedpassword", "salt")).Returns(false);

            // Act
            var user = _userStore.ValidateUser("testuser", "wrongpassword");

            // Assert
            Assert.Null(user);
        }
    }
}
