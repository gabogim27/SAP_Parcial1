namespace SAP.Crypto.Domain.Request
{
    using SAP.Crypto.Domain.Implementation;
    using System;

    public class TransferDepositRequest
    {
        public string CBU { get; set; }
        public string Alias { get; set; }
        public decimal Amount { get; set; }
        public Guid UUID { get; set; }
        public string SourceDni { get; set; }
        public AccountType AccountType { get; set; }
    }
}
