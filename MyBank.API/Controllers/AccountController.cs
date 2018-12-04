using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBank.API.Data;
using MyBank.API.Dtos;
using MyBank.API.Models;

namespace MyBank.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAcctRepository _repo;

        public AccountController(DataContext context, IAcctRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetAccounts(int userID)
        {
            var identity = User.FindFirst(ClaimTypes.NameIdentifier);
            var claimsID = identity.Value;
            if(userID != int.Parse(claimsID))
            {
                return Unauthorized();
            }
            var accounts = await _repo.GetAccounts(int.Parse(claimsID));
            if(accounts == null)
            {
                return BadRequest("No accounts associated with this ID");
            }
            else
            {
                return Ok(accounts);
            }
        }

        [HttpGet("get-percentage-breakdowns")]
        public async Task<IActionResult> GetPercentageBreakdowns(GetPercentageDto percent)
        {
        //check that the user is authorized to view these percentage breakdowns
            var foundAccount = await _repo.FindAccount(percent.accountID);
            var identity = User.FindFirst(ClaimTypes.NameIdentifier);
            if(identity == null)
            {
                return NotFound();
            }
            var claimsID = identity.Value;
            if(foundAccount.userID != int.Parse(claimsID))
            {
                return Unauthorized();
            }
        //get the percentage breakdowns
            var percentageBreakdowns = await _repo.GetPercentageBreakdowns(percent.accountID);
            if(percentageBreakdowns == null)
            {
                return BadRequest("No percentage breakdowns for this account.");
            }
            return Ok(percentageBreakdowns);
        }

        [HttpGet("get-transaction-history")]
        public async Task<IActionResult> GetTransactionHistory(GetHistoryDto history)
        {
        //check that the user is authorized to view these histories
            var foundAccount = await _repo.FindAccount(history.accountID);
            var identity = User.FindFirst(ClaimTypes.NameIdentifier);
            if(identity == null)
            {
                return NotFound();
            }
            var claimsID = identity.Value;
            if(foundAccount.userID != int.Parse(claimsID))
            {
                return Unauthorized();
            }
        //get the account histories
            var histories = await _repo.GetTransactionHistories(history.accountID);
            if(histories == null)
            {
                return BadRequest("No transaction histories for this account.");
            }
            return Ok(histories);
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount(AccountForRegisterDto newAccount)
        {
        //get the userID to associate with the account
            var identity = User.FindFirst(ClaimTypes.NameIdentifier);
            if(identity == null)
            {
                return NotFound();
            }
            var claimsID = identity.Value;
        //create the new account
            var accountToCreate = new Account
            {
                accountName = newAccount.accountName,
                accountTotal = newAccount.accountTotal,
                accountPercent = 1.00m,
                userID = int.Parse(claimsID)

            };
            
            var createdUser = await _repo.CreateAccount(accountToCreate);
            return Ok(201);
        }

        [HttpPost("create-percentage")]
        public async Task<IActionResult> AddPercentage(PercentageCreationDto newPercentage)
        {
        //Check first if the account belongs to the current user logged in.
        //find the account that is requesting the change.
            var foundAccount = await _repo.FindAccount(newPercentage.accountID); 
        //get the user id from the JWT.
            var identity = User.FindFirst(ClaimTypes.NameIdentifier); 
            if(identity == null)
            {
                return NotFound();
            }
            var claimID = identity.Value;
        //check that the id from the jwt matches the user id of the account requesting the change.
            if(foundAccount.userID != int.Parse(claimID)) 
            {
                return Unauthorized();
            }
        //attempt to update the account's percentage
            if(! await _repo.UpdateAccountPercentage(newPercentage.accountID, newPercentage.percentageAmount))
            {
                return BadRequest("Requested Percentage greater than the percentage remaining on this account.");
            }
        //if account's percentage can be updated, add newPercent to the table. 
            else
            {
                decimal newPercentageTotal = foundAccount.accountTotal * (newPercentage.percentageAmount/100);
                var newPercent = new PercentageBreakdown
                {
                    accountID = newPercentage.accountID,
                    PercentageBreakdownName = newPercentage.percentageName,
                    PercentageAmount = newPercentage.percentageAmount,
                    PercentageTotal = newPercentageTotal
                };
                await _repo.CreatePercentageBreakdown(newPercent);
            }
            return Ok(201);
        }

        [HttpPost("create-set-amount")]
        public async Task<IActionResult> CreateSetAmount(SetAmountCreationDto newSetAmount)
        {
        //Check first if the account belongs to the current user logged in.
        //find the account that is requesting the change.
            var foundAccount = await _repo.FindAccount(newSetAmount.accountID); 
        //get the user id from the JWT.
            var identity = User.FindFirst(ClaimTypes.NameIdentifier); 
            if(identity == null)
            {
                return NotFound();
            }
            var claimID = identity.Value;
        //check that the id from the JWT matches the user id of the account requesting the change.
            if(foundAccount.userID != int.Parse(claimID)) 
            {
                return Unauthorized();
            }
            return Ok(201);
        }

        [HttpPost("add-funds")]
        public async Task<IActionResult> AddFunds(AddFundsToAccountDto fundsToAdd)
        {
        //make sure the account that the user is attempting to alter belongs to that user.
            var foundAccount = await _repo.FindAccount(fundsToAdd.accountID);
            var identity = User.FindFirst(ClaimTypes.NameIdentifier);
            if(identity == null)
            {
                return NotFound();
            }
            var claimsID = identity.Value;
            if(foundAccount.userID != int.Parse(claimsID))
            {
                return Unauthorized();
            }
        //update the account total
            decimal newTotal = await _repo.AddToAccountTotal(fundsToAdd.accountID, fundsToAdd.amountToBeAdded);
        //recalculate the percentages
            await _repo.UpdatePercentageBreakdowns(fundsToAdd.accountID, newTotal);
        //create transaction history
            var newTransactionHistory = new TransactionHistory
            {
                transactionDateTime = DateTime.Now,
                transactionType = fundsToAdd.transactionType,
                TransactionDetail = fundsToAdd.transactionDetail,
                accountID = fundsToAdd.accountID
            };
            await _repo.CreateHistory(newTransactionHistory);
            return Ok(200);
        }

        [HttpPost("remove-funds")]
        public async Task<IActionResult> RemoveFunds(RemoveFundsFromAccountDto fundsToBeRemoved)
        {
        //make sure the account that the user is attempting to alter belongs to that user.
            var foundAccount = await _repo.FindAccount(fundsToBeRemoved.accountID);
            if(foundAccount == null)
            {
                return NotFound();
            }
            var identity = User.FindFirst(ClaimTypes.NameIdentifier);
            decimal newTotal = 0.00m;
            if(identity == null)
            {
                return NotFound();
            }
            var claimsID = identity.Value;
            if(foundAccount.userID != int.Parse(claimsID))
            {
                return Unauthorized();
            }
        //check if the funds are to be removed from the total, or from a certain percentage.
            if(fundsToBeRemoved.percentageID == 0)
            {
            //the amount is to be removed from the grand total
                newTotal = await _repo.RemoveFromAccountTotal(fundsToBeRemoved.accountID, fundsToBeRemoved.amountToBeRemoved);
                await _repo.UpdatePercentageBreakdowns(fundsToBeRemoved.accountID, newTotal);
            }
            else
            {
            //The amount is to be removed from the percentage
            //remove the amount from the single percentage breakdown
                await _repo.RemoveFundsFromSinglePercentageBreakdown(fundsToBeRemoved.percentageID, fundsToBeRemoved.amountToBeRemoved);
                newTotal = await _repo.RemoveFromAccountTotal(fundsToBeRemoved.accountID, fundsToBeRemoved.amountToBeRemoved);
            }
        //create transaction history
            var newTransactionHistory = new TransactionHistory
            {
                transactionDateTime = DateTime.Now,
                transactionType = fundsToBeRemoved.transactionType,
                TransactionDetail = fundsToBeRemoved.transactionDetail,
                accountID = fundsToBeRemoved.accountID
            };
            await _repo.CreateHistory(newTransactionHistory);
            return Ok(200);
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount(AccountToDeleteDto deleteAccount)
        {
        //make sure the account that the user is attempting to alter belongs to that user.
            var foundAccount = await _repo.FindAccount(deleteAccount.accountID);
            var identity = User.FindFirst(ClaimTypes.NameIdentifier);
            if(identity == null)
            {
                return NotFound();
            }
            var claimsID = identity.Value;
            if(foundAccount.userID != int.Parse(claimsID))
            {
                return Unauthorized();
            }
        //delete the account
            await _repo.DeleteAccount(deleteAccount.accountID);
        //delete the percentage breakdowns associated with that account
            await _repo.DeletePercentageBreakdowns(deleteAccount.accountID);
        //delete transaction histories associated with that account
            await _repo.DeleteTransactionHistory(deleteAccount.accountID);
            return Ok(200);
        }

        [HttpDelete("delete-single-percentage")]
        public async Task<IActionResult> DeleteSinglePercentage(DeletePercentageDto percentageToDelete)
        {
        //make sure the account that the user is attempting to alter belongs to that user.
            var foundPercent = await _repo.FindPercentBreakdown(percentageToDelete.percentID);
            var foundAccount = await _repo.FindAccount(foundPercent.accountID);
            var identity = User.FindFirst(ClaimTypes.NameIdentifier);
            if(identity == null)
            {
                return NotFound();
            }
            var claimsID = identity.Value;
            if(foundAccount.userID != int.Parse(claimsID))
            {
                return Unauthorized();
            }
        //delete the requested percentage
            await _repo.DeleteSinglePercentage(foundPercent);
            return Ok(200);
        }
    }
}