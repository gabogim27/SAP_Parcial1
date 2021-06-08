namespace SAP.Crypto.Domain.Implementation
{
    using System.ComponentModel.DataAnnotations;

    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }

        public decimal Price { get; set; }

        public AccountType Type { get; set; }
    }
}
