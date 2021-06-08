namespace SAP.Crypto.Domain.Response
{
    using SAP.Crypto.Domain.Implementation;
    using System;
    using System.Collections.Generic;

    public class AccountResponse
    {
        public int AccountNumber { get; set; }

        public Guid UUID { get; set; }

        public Bank BankInfo { get; set; } = new Bank();

        public string CBU { get; set; }

        public string Alias { get; set; }

        public decimal ArsBalance { get; set; }

        public decimal UsdBalance { get; set; }

        public decimal CryptoBalance { get; set; }

        public CryptoType CryptoType { get; set; }

        public Customer Customer { get; set; } = new Customer();

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
