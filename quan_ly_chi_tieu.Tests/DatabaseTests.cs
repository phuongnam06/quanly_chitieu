using Microsoft.EntityFrameworkCore;
using quan_ly_chi_tieu.Data;
using quan_ly_chi_tieu.Models;
using Xunit;

namespace quan_ly_chi_tieu.Tests
{
    public class DatabaseTests
    {
        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddUser_ShouldBeStoredInDatabase()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var user = new User 
            { 
                Id = Guid.NewGuid(), 
                Email = "test@example.com", 
                Username = "testuser",
                PasswordHash = "hashed_pass" 
            };

            // Act
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var storedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
            Assert.NotNull(storedUser);
            Assert.Equal("testuser", storedUser.Username);
        }
    }
}
