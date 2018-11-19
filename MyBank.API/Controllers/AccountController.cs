using System.Collections.Generic;
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

        [HttpGet]
        public async Task<IActionResult> GetAccounts(int userID)
        {
            var accounts = await _repo.GetAccounts(userID);
            return Ok(accounts);
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount(AccountForRegisterDto newAccount, int currentUser)
        {
            var accountToCreate = new Account
            {
                accountName = newAccount.accountName,
                accountTotal = newAccount.accountTotal,
                accountPercent = 1,
                userID = currentUser

            };
            
            var createdUser = await _repo.CreateAccount(accountToCreate);
            return Ok(201);
        }
    }
}