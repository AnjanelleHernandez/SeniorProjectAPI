using System;
using System.Threading.Tasks;
using MyBank.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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

        public async Task<Account> CreateAccount(Account newAccount)
        {
            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return newAccount;
        }

        public async Task<IEnumerable<Account>> GetAccounts(int currentUser)
        {
            var accounts = await _context.Accounts.Where(c => c.userID == currentUser).ToListAsync();
            return accounts;
        }
    }

}