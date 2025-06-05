using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class UserService
    {
        private readonly LibraryDbContext _context;

        public UserService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task VerifyRegisterAsync(UserForRegister newUser, string oid)
        {
            if (!string.IsNullOrEmpty(oid))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.ObjectId == oid);
                if (user != null)
                {
                    user.UserName = newUser.UserName;
                    user.DateOfBirth = newUser.DateOfBirth;
                    user.Phone = newUser.Phone;
                    user.Address = newUser.Address;
                    user.City = newUser.City;
                    user.Postalcode = newUser.Postalcode;
                    user.RegistrationComplete = true;

                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> CheckUserInDb(string? oid)
        {
            string isNull = "0";

            if (!string.IsNullOrEmpty(oid))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.ObjectId == oid);
                if (user == null)
                {
                    return false;
                }

                if (user.Address == isNull || user.City == isNull || user.Phone == isNull || user.Postalcode == isNull || user.RegistrationComplete == false)
                {
                    return false;
                }
                return true;
            }
            return false;
        }


        public async Task<UserEntity?> GetUserInformation(string? oid)
        {
            if (!string.IsNullOrEmpty(oid))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.ObjectId == oid);
                Console.WriteLine(user);
                return user;
            }

            return null;
        }

        public async Task<bool> UpdateUserInfo(UserForDataUpdate userToUpdate, string? oid)
        {
            if (!string.IsNullOrEmpty(oid))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.ObjectId == oid);
                if (user != null)
                {
                    user.UserName = userToUpdate.UserName;
                    user.Phone = userToUpdate.Phone;
                    user.Address = userToUpdate.Address;
                    user.City = userToUpdate.City;
                    user.Postalcode = userToUpdate.Postalcode;

                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> CheckRegistrationState(string? oid)
        {
            if (!string.IsNullOrEmpty(oid))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.ObjectId == oid);
                if (user == null)
                {
                    return false;
                } else if (user.RegistrationComplete == false)
                {
                    return false;
                } else
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> SetRegistrationStateToTrue(string? oid)
        {
            if (!string.IsNullOrEmpty(oid))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.ObjectId == oid);
                if (user == null)
                {
                    return false;
                }
                
                if (user.RegistrationComplete == false)
                {
                    user.RegistrationComplete = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
