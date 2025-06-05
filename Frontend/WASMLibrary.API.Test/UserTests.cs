using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace WASMLibrary.Api.Test
{
    public class UserTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly LibraryDbContext _context;

        public UserTests(WebApplicationFactory<Program> factory, LibraryDbContext context)
        {
            _factory = factory;
            _context = context;
        }

        [Fact]
        public async Task VerifyRegisterTest()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var originalUser = new UserEntity
            {
                ObjectId = "abc123",
                UserName = "AltName",
                DateOfBirth = new DateTime(1990, 1, 1),
                Phone = "000000",
                Address = "Alte Straﬂe 1",
                City = "Altstadt",
                Postalcode = "12345"
            };

            _context.Users.Add(originalUser);
            _context.SaveChanges();

            var newUser = new UserForRegister
            {
                UserName = "NewUser",
                Phone = "+41799999999",
                DateOfBirth = new DateTime(2002, 11, 12),
                Address = "Grubenstrasse 37",
                Postalcode = "4900",
                City = "Langenthal"
            };

            await userService.VerifyRegisterAsync(newUser, "abc123");

            var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.ObjectId == "abc123");

            Assert.NotNull(updatedUser);
            Assert.Equal("ibrazqrj2", updatedUser.Name);
            Assert.Equal(new DateTime(2000, 12, 01), updatedUser.DateOfBirth);
            Assert.Equal("+41799532744", updatedUser.Phone);
            Assert.Equal("Falkenstrasse 40b", updatedUser.Address);
            Assert.Equal("Langenthal", updatedUser.City);
            Assert.Equal("4900", updatedUser.Postalcode);
        }

        [Fact]
        public async Task CheckUserInDbTest()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var result = await userService.CheckUserInDb("abc123");

            Assert.True(result);
        }

        [Fact]
        public async Task GetUserInformationTest()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var result = await userService.GetUserInformation("abc123");

            Assert.NotNull(result);
            Assert.Equal("ibrazqrj2", result.Name);
            Assert.Equal(new DateTime(2000, 12, 01), result.DateOfBirth);
            Assert.Equal("+41799532744", result.Phone);
            Assert.Equal("Falkenstrasse 40b", result.Address);
            Assert.Equal("Langenthal", result.City);
            Assert.Equal("4900", result.Postalcode);
        }

        [Fact]
        public async Task UpdateUserInfoTest()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var userToUpdate = new UserForDataUpdate
            {
                UserName = "AltName",
                Phone = "000000",
                Address = "Alte Straﬂe 1",
                City = "Altstadt",
                Postalcode = "12345"
            };

            await userService.UpdateUserInfo(userToUpdate, "abc123");
        }
    }
}