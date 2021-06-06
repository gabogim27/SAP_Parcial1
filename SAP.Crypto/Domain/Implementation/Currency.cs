namespace SAP.Crypto.Domain.Implementation
{
    using SAP.Crypto.Domain.Implementation;
    using System.ComponentModel.DataAnnotations;

    public class Currency
    {
        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        public AccountType Type { get; set; }
    }
}
