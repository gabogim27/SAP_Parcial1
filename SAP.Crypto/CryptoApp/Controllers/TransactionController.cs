namespace CryptoApp.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using SAP.Crypto.Repository.Interfaces;
    using System.Transactions;

    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        //public TransactionController(IRepository<Transaction> repository)
        //{
        //    this.repository = repository;
        //}

        //[HttpGet]
        //[Route("transactions")]
        //[Produces(typeof(IEnumerable<Transaction>))]
        //public IActionResult GetTransactions()
        //{
        //    var transactions = repository.List();
        //    return Ok(transactions);
        //}
    }
}
