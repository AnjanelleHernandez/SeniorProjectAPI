using System.Threading.Tasks;
using MyBank.API.Models;

namespace MyBank.API.Data
{
    public interface IAcctRepository
    {
        Task<Account> CreateAccount(Account newAccount, int currentUser); 
        Task<bool> UpdatePercentage(int accountID, decimal percentage);

    }
}