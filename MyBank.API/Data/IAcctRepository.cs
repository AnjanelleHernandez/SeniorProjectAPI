using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyBank.API.Models;

namespace MyBank.API.Data
{
    public interface IAcctRepository
    {
        Task<Account> CreateAccount(Account newAccount); 
        Task<bool> UpdateAccountPercentage(int accountID, decimal percentage);
        Task<IEnumerable<Account>> GetAccounts(int currentUser);
        Task<Account> FindAccount(int accountID);
        Task<PercentageBreakdown> CreatePercentageBreakdown(PercentageBreakdown newPercentageBreakdown);
        Task<IEnumerable<PercentageBreakdown>> GetPercentageBreakdowns(int accountID);
        Task<decimal> AddToAccountTotal(int accountID, decimal addedFunds);
        Task<decimal> RemoveFromAccountTotal(int accountID, decimal removedFunds);
        Task UpdatePercentageBreakdowns(int accountID, decimal NewAccountTotal);
        Task RemoveFundsFromSinglePercentageBreakdown(int percentID, decimal fundsToBeRemoved);
        Task<TransactionHistory> CreateHistory(TransactionHistory newHistory);
        Task<IEnumerable<TransactionHistory>> GetTransactionHistories(int accountID);
        Task DeleteAccount(int accountID);
        Task DeleteTransactionHistory(int accountID);
        Task DeletePercentageBreakdowns(int accountID);
        Task<PercentageBreakdown> FindPercentBreakdown(int percentID);
        Task DeleteSinglePercentage(PercentageBreakdown percentageToDelete);
    }
}