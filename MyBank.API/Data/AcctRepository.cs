using System;
using System.Threading.Tasks;
using MyBank.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MyBank.API.Data
{
    public class AcctRepository : IAcctRepository
    {
        private readonly DataContext _context;

        public AcctRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdatePercentage(int accountID, decimal percentage)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.accountID == accountID);
            decimal newPercentage = account.accountPercent - (percentage / (decimal)100);
            if(newPercentage < 0)
            {
                return false;
            }
            else
            {
                account.accountPercent = newPercentage;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Account> CreateAccount(Account newAccount, int currentUser)
        {
            newAccount.userID = currentUser;
            await _context.Accounts.AddAsync(newAccount);
            return newAccount;
        }
    }
}