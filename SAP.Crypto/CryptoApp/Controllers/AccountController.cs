namespace CryptoApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SAP.Crypto.Domain.Implementation;
    using SAP.Crypto.Domain.Request;
    using SAP.Crypto.Domain.Response;
    using SAP.Crypto.Services.Interfaces;
    using System;

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("Add")]
        [Produces(typeof(int))]
        public IActionResult CreateAccount(AccountRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.DNI) ||
                string.IsNullOrWhiteSpace(request.BankName) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest();
            }

            try
            {
                var accountNumber = accountService.CreateAccount(request);

                return Ok(accountNumber);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("Get")]
        [Produces(typeof(AccountResponse))]
        public IActionResult GetAccount(AccountIdRequest idRequest)
        {
            if ((idRequest.AccountType == AccountType.Crypto && idRequest.UUID == null) ||
                (idRequest.AccountType == AccountType.ARSAndDollars && idRequest.AccountNumber <= 0))
            {
                return BadRequest();
            }

            try
            {
                var account = accountService.GetAccount(idRequest);

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("HasAccount")]
        [Produces(typeof(bool))]
        public IActionResult HasAccount(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return BadRequest();
            }

            try
            {
                var hasAccount = accountService.HasAccount(dni);

                return Ok(hasAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("Deposit")]
        [Produces(typeof(bool))]
        public IActionResult Deposit(TransferDepositRequest request)
        {
            if ((string.IsNullOrWhiteSpace(request.CBU) && 
                request.Amount <= 0 && 
                string.IsNullOrWhiteSpace(request.SourceDni)) ||
                (string.IsNullOrWhiteSpace(request.Alias) &&
                request.Amount <= 0 &&
                string.IsNullOrWhiteSpace(request.SourceDni)))
            {
                return BadRequest();
            }

            try
            {
                var success = accountService.Deposit(request);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("Transfer")]
        [Produces(typeof(bool))]
        public IActionResult Transfer(TransferDepositRequest request)
        {
            if ((string.IsNullOrWhiteSpace(request.CBU) &&
                request.Amount <= 0 &&
                string.IsNullOrWhiteSpace(request.SourceDni)) ||
                (string.IsNullOrWhiteSpace(request.Alias) &&
                request.Amount <= 0 &&
                string.IsNullOrWhiteSpace(request.SourceDni)))
            {
                return BadRequest();
            }

            try
            {
                var success = accountService.Transfer(request);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("BuyCrypto")]
        [Produces(typeof(bool))]
        public IActionResult BuyCrypto(TransferDepositRequest request)
        {
            if ((string.IsNullOrWhiteSpace(request.CBU) &&
                request.Amount <= 0 &&
                string.IsNullOrWhiteSpace(request.SourceDni)) ||
                (string.IsNullOrWhiteSpace(request.Alias) &&
                request.Amount <= 0 &&
                string.IsNullOrWhiteSpace(request.SourceDni)))
            {
                return BadRequest();
            }

            try
            {
                var success = accountService.BuyCrypto(request);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
