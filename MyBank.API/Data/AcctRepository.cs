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

        public async Task<bool> UpdateAccountPercentage(int accountID, decimal requestedPercentage)
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

        public async Task<decimal> AddToAccountTotal(int accountID, decimal addedFunds)
        {
            var foundAccount = await FindAccount(accountID);
            foundAccount.accountTotal += addedFunds;
            await _context.SaveChangesAsync();
            return foundAccount.accountTotal;
        }

        public async Task<decimal> RemoveFromAccountTotal(int accountID, decimal removedFunds)
        {
            var foundAccount = await FindAccount(accountID);
            foundAccount.accountTotal -= removedFunds;
            await _context.SaveChangesAsync();
            return foundAccount.accountTotal;
        }
        public async Task<IEnumerable<PercentageBreakdown>> GetPercentageBreakdowns(int accountID)
        {
            var accountPercentages = await _context.PercentageBreakdowns.Where(x => x.accountID == accountID).ToListAsync();
            return accountPercentages;
        }

        public async Task<IEnumerable<TransactionHistory>> GetTransactionHistories(int accountID)
        {
            var accountHistories = await _context.TransactionHistories.Where(x => x.accountID == accountID).ToListAsync();
            return accountHistories;
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

        public async Task UpdatePercentageBreakdowns(int accountID, decimal newAccountTotal)
        {
            var foundAccount = await FindAccount(accountID);
            var accountBreakdowns = await GetPercentageBreakdowns(accountID);
            foreach (PercentageBreakdown percentBreakdown in accountBreakdowns)
            {
                await RecalculatePercentages(percentBreakdown, newAccountTotal);
            }
        }

        public async Task<TransactionHistory> CreateHistory(TransactionHistory newHistory)
        {
            await _context.TransactionHistories.AddAsync(newHistory);
            await _context.SaveChangesAsync();
            return newHistory;
        }
        public async Task RemoveFundsFromSinglePercentageBreakdown(int percentID, decimal fundsToBeRemoved)
        {
            var percentBreakdown = await _context.PercentageBreakdowns.FirstOrDefaultAsync(x => x.PercentageBreakdownID == percentID);
            percentBreakdown.PercentageTotal -= fundsToBeRemoved;
            await _context.SaveChangesAsync();        
        }

        public async Task DeleteAccount(int accountID)
        {
            var accountToDelete = await FindAccount(accountID);
            _context.Accounts.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePercentageBreakdowns(int accountID)
        {
            var breakdowns = await GetPercentageBreakdowns(accountID);
            foreach(PercentageBreakdown deletePercent in breakdowns)
            {
                _context.PercentageBreakdowns.Remove(deletePercent);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteTransactionHistory(int accountID)
        {
            var transHistToDelete = await GetTransactionHistories(accountID);
            foreach(TransactionHistory history in transHistToDelete)
            {
                _context.TransactionHistories.Remove(history);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSinglePercentage(PercentageBreakdown percentageToDelete)
        {
        //add the set aside percentage back to the total percentage.
            Account acctToUpdate = await FindAccount(percentageToDelete.accountID);
            acctToUpdate.accountPercent += (percentageToDelete.PercentageAmount / 100);
        //remove the set aside percentage from the database.
            _context.PercentageBreakdowns.Remove(percentageToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<PercentageBreakdown> FindPercentBreakdown(int percentID)
        {
            PercentageBreakdown foundPercent = await _context.PercentageBreakdowns.FirstOrDefaultAsync(x => x.PercentageBreakdownID == percentID);
            return foundPercent;
        }
        private async Task RecalculatePercentages(PercentageBreakdown breakdownToUpdate, decimal newAccountTotal)
        {
            breakdownToUpdate.PercentageTotal = Math.Round(newAccountTotal * (breakdownToUpdate.PercentageAmount / 100), 2);
            await _context.SaveChangesAsync();
        }
    }

}