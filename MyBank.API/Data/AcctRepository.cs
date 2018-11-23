using System;
using System.Threading.Tasks;
using MyBank.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Security.Claims;

namespace MyBank.API.Data
{
    public class AcctRepository : IAcctRepository
    {
        private readonly DataContext _context;

        public AcctRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdatePercentage(int accountID, decimal requestedPercentage)
        {
        //find the account with the account ID to make the changes.
            Account account = await FindAccount(accountID);
        //calculate the new percentage with the account's current percent - the requested percentage
            decimal newPercentage = account.accountPercent - (requestedPercentage / (decimal)100);
        //check that the new percentage is <= 0.
            if(newPercentage < 0) 
            {
                return false;
            }
        //update the account percent if new percentage is >= 0;
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


        public async Task<Account> FindAccount(int accountID)
        {
            var accountFound = await _context.Accounts.FirstOrDefaultAsync(x => x.accountID == accountID);
            return accountFound;
        }

        public async Task<PercentageBreakdown> CreatePercentageBreakdown(PercentageBreakdown newPercentageBreakdown)
        {
            await _context.PercentageBreakdowns.AddAsync(newPercentageBreakdown);
            await _context.SaveChangesAsync();
            return newPercentageBreakdown;
        }
    }

}