using System.Collections.Generic;
using System.Threading.Tasks;
using MyBank.API.Models;

namespace MyBank.API.Data
{
    public interface IAcctRepository
    {
        Task<Account> CreateAccount(Account newAccount); 
        Task<bool> UpdatePercentage(int accountID, decimal percentage);
        Task<IEnumerable<Account>> GetAccounts(int currentUser);

    }
}