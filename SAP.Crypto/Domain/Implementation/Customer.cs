namespace SAP.Crypto.Domain.Implementation
{
    using System.ComponentModel.DataAnnotations;
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string DNI { get; set; }
    }
}
