using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.ComponentModel.DataAnnotations;

namespace WASMLibrary.Client.Test
{
    public class UserTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UserTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        // Tests with Valid values
        [Fact]
        public async Task VerifyRegisterTest()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var originalUser = new UserEntity
            {
                ObjectId = "111abc",
                Name = "Alt",
                UserName = "AltName",
                EMail = "alt@name.ch",
                DateOfBirth = new DateTime(1990, 1, 1),
                Phone = "000000",
                Address = "Alte Straﬂe 1",
                City = "Altstadt",
                Postalcode = "12345"
            };

            context.Users.Add(originalUser);
            context.SaveChanges();

            var newUser = new UserForRegister
            {
                UserName = "NewUser123",
                Phone = "+41799999999",
                DateOfBirth = new DateTime(2002, 11, 12),
                Address = "Grubenstrasse 37",
                Postalcode = "4900",
                City = "Langenthal"
            };

            await userService.VerifyRegisterAsync(newUser, "111abc");

            var updatedUser = await context.Users.FirstOrDefaultAsync(u => u.ObjectId == "111abc");

            Assert.Equal("NewUser123", updatedUser.UserName);
            Assert.Equal(new DateTime(2002, 11, 12), updatedUser.DateOfBirth);
            Assert.Equal("+41799999999", updatedUser.Phone);
            Assert.Equal("Grubenstrasse 37", updatedUser.Address);
            Assert.Equal("Langenthal", updatedUser.City);
            Assert.Equal("4900", updatedUser.Postalcode);
            Assert.Equal("Alt", updatedUser.Name);
            Assert.Equal("alt@name.ch", updatedUser.EMail);
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
            Assert.Equal("Alt", result.Name);
            Assert.Equal("AltName", result.UserName);
            Assert.Equal(new DateTime(2002, 11, 12), result.DateOfBirth);
            Assert.Equal("000000", result.Phone);
            Assert.Equal("Alte Straﬂe 1", result.Address);
            Assert.Equal("Altstadt", result.City);
            Assert.Equal("12345", result.Postalcode);
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

        //Tests with invalid values
        [Fact]
        public async Task VerifyRegisterTest_WithInvalidOid_DoesNothing()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var newUser = new UserForRegister
            {
                UserName = "Unused",
                Phone = "000000",
                DateOfBirth = new DateTime(2000, 1, 1),
                Address = "Unknown",
                Postalcode = "0000",
                City = "Nowhere"
            };

            await userService.VerifyRegisterAsync(newUser, "nonexistent_oid");

            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Unused");

            Assert.Null(user);
        }

        [Fact]
        public async Task CheckUserInDB_WithInvalidId_ReturnsFalse()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var result = await userService.CheckUserInDb("nonexistent");

            Assert.False(result);
        }

        [Fact]
        public async Task GetUserInformation_NonexistentId_ReturnsNull()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var result = await userService.GetUserInformation("doesnotexist");

            Assert.Null(result);
        }

        [Fact]
        public async Task VerifyRegister_WithOverFiftyCharsInUserName_Validation()
        {
            var user = new UserEntity
            {
                ObjectId = "abc123",
                Name = "Max",
                UserName = new string('A', 51),
                EMail = "test@mail.com",
                Phone = "123456",
                DateOfBirth = new DateTime(2000, 1, 1),
                Address = "Teststrasse 1",
                Postalcode = "12345",
                City = "Teststadt"
            };

            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(user, context, results, validateAllProperties: true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserEntity.UserName)));
        }

        [Fact]
        public async Task VerifyRegister_WithOverTwentyNumbersInPostalcode_Validation()
        {
            var user = new UserEntity
            {
                ObjectId = "abc123",
                Name = "Max",
                UserName = "MaxTest",
                EMail = "test@mail.com",
                Phone = "123456",
                DateOfBirth = new DateTime(2000, 1, 1),
                Address = "Teststrasse 1",
                Postalcode = "1234512345123451234512345",
                City = "Teststadt"
            };

            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(user, context, results, validateAllProperties: true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserEntity.Postalcode)));
        }

        [Fact]
        public async Task VerifyRegister_WithOverTwentyCharsInPhone_Validation()
        {
            var user = new UserEntity
            {
                ObjectId = "abc123",
                Name = "Max",
                UserName = "MaxTest",
                EMail = "test@mail.com",
                Phone = "123456123456123456123456123456123456",
                DateOfBirth = new DateTime(2000, 1, 1),
                Address = "Teststrasse 1",
                Postalcode = "12345",
                City = "Teststadt"
            };

            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(user, context, results, validateAllProperties: true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserEntity.Phone)));
        }

        [Fact]
        public async Task VerifyRegister_WithOverSixtyCharsInEMail_Validation()
        {
            var user = new UserEntity
            {
                ObjectId = "abc123",
                Name = "Max",
                UserName = "MaxTest",
                EMail = "testtesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttest@mail.com",
                Phone = "123456",
                DateOfBirth = new DateTime(2000, 1, 1),
                Address = "Teststrasse 1",
                Postalcode = "12345",
                City = "Teststadt"
            };

            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(user, context, results, validateAllProperties: true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserEntity.EMail)));
        }

        [Fact]
        public async Task GetUserInformation_WithWrongOid()
        {
            using var scope = _factory.Services.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var result = await userService.GetUserInformation(null);

            Assert.Null(result);
        }
    }
}