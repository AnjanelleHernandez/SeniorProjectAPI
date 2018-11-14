using System.Threading.Tasks;
using MyBank.API.Models;

namespace MyBank.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string emailAddress, string password);
         Task<bool> UserExists(string emailAddress);

    }
}