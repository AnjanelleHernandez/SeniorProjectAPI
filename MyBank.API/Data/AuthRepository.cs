using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBank.API.Models;

namespace MyBank.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string emailAddress, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.emailAddress == emailAddress);

            if(user == null)
            {
                return null;
            }
            if(!VerifyPasswordHash(password, user.passwordHash, user.passwordSalt))
            {
                return null;
            }
            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string emailAddress)
        {
            if (await _context.Users.AnyAsync(x => x.emailAddress == emailAddress))
            {
                return true; //Email address is associated with a user in the database.
            }
            return false; //Email address is not associated with a user in the database.
        }

        //verify that the password that it attempting to log in is correct.
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
             using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        //hash the password so it is not stored in the database as plain text.
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }
    }
}