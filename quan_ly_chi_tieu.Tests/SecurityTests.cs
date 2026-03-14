using Xunit;
using BCrypt.Net;

namespace quan_ly_chi_tieu.Tests
{
    public class SecurityTests
    {
        [Fact]
        public void BCrypt_HashPassword_And_Verify_ShouldWork()
        {
            // Arrange
            string password = "TestPassword123";

            // Act
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            bool isValid = BCrypt.Net.BCrypt.Verify(password, hash);

            // Assert
            Assert.True(isValid);
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void BCrypt_Verify_WithWrongPassword_ShouldReturnFalse()
        {
            // Arrange
            string password = "TestPassword123";
            string wrongPassword = "WrongPassword";
            string hash = BCrypt.Net.BCrypt.HashPassword(password);

            // Act
            bool isValid = BCrypt.Net.BCrypt.Verify(wrongPassword, hash);

            // Assert
            Assert.False(isValid);
        }
    }
}
