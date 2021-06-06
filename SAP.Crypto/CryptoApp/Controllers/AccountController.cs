namespace CryptoApp.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using SAP.Crypto.Domain.Implementation;
    using SAP.Crypto.Repository.Interfaces;
    using System.Transactions;

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRepository<BankAccount> bankRepository;

        private readonly IRepository<CryptoAccount> crytpoRepository;

        public AccountController(IRepository<BankAccount> bankRepository, IRepository<CryptoAccount> crytpoRepository)
        {
            this.bankRepository = bankRepository;
            this.crytpoRepository = crytpoRepository;
        }

        [HttpGet]
        [Route("Accountants")]
        [Produces(typeof(IEnumerable<Transaction>))]
        public IActionResult GetTransactions()
        {
            var transactions = repository.List();
            return Ok(transactions);
        }
    }
}
