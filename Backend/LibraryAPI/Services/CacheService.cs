using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace LibraryAPI.Services
{
    public class CacheService
    {
        private readonly List<string> _knownUsers = new List<string>();
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CacheService> _logger;

        private readonly object _syncLock = new();

        public CacheService(IServiceScopeFactory scopeFactory, ILogger<CacheService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public bool IsKnownUser(string objectId)
        {
            lock (_syncLock)
            {
                bool exists = _knownUsers.Contains(objectId);
                if(!exists) _knownUsers.Add(objectId);
                return exists;
            }
        }

        public async Task VerifyLoginAsync(string oid, string name, string email)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            if (!string.IsNullOrEmpty(oid) && !_knownUsers.Contains(oid) && !IsKnownUser(oid))
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.ObjectId == oid);
                if (user == null)
                {
                    user = new UserEntity
                    {
                        ObjectId = oid,
                        Name = name!,
                        EMail = email!,
                        UserName = string.Join(".", name!.ToLower().Split(" ")),
                        Address = string.Empty,
                        City = string.Empty,
                        DateOfBirth = DateTime.Today,
                        Phone = string.Empty,
                        Postalcode = string.Empty,
                        RegistrationComplete = false
                    };
                    db.Users.Add(user);
                    _logger.LogInformation("addUser");
                    await db.SaveChangesAsync();
                }
            }
            else if (!string.IsNullOrEmpty(oid) && _knownUsers.Contains(oid))
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.ObjectId == oid);
                if (user != null)
                {
                    user.Name = name;
                    user.EMail = email;
                    await db.SaveChangesAsync();
                }
            }
        }


    }
}
