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
            if(! await _repo.UpdatePercentage(newPercentage.accountID, newPercentage.percentageAmount))
            {
                return BadRequest("Requested Percentage greater than the percentage remaining on this account.");
            }
        //if account's percentage can be updated, add newPercent to the table. 
            else
            {
                decimal newPercentageTotal = foundAccount.accountTotal * newPercentage.percentageAmount;
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
    }
}