namespace SAP.Crypto.Repository.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using SAP.Crypto.Domain.Implementation;
    using System;

    public class DatabaseContext : DbContext
    {
        //por cada entidad ir agregando un set distinto
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<CryptoAccount> CryptoAccount { get; set; }
        public DbSet<Customer> Customer { get; set; }


        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging()
                          .EnableDetailedErrors()
                          .LogTo(Console.WriteLine);
        }
    }
}
