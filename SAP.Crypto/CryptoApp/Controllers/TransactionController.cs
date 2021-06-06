using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Domain;
namespace CryptoApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SAP.Crypto.Repository.Interfaces;
    using System.Transactions;

    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IRepository<Transaction> repository;

        public TransactionController(IRepository<Transaction> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("transactions")]
        [Produces(typeof(IEnumerable<Transaction>))]
        public IActionResult GetTransactions()
        {
            var transactions = repository.List();
            return Ok(transactions);
        }
    }
}
