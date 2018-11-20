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
            var identity = User.FindFirst(ClaimTypes.NameIdentifier);
            var claimsID = identity.Value;
            var accountToCreate = new Account
            {
                accountName = newAccount.accountName,
                accountTotal = newAccount.accountTotal,
                accountPercent = 1,
                userID = int.Parse(claimsID)

            };
            
            var createdUser = await _repo.CreateAccount(accountToCreate);
            return Ok(201);
        }
    }
}