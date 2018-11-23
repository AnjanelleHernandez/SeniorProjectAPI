using MyBank.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MyBank.API.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Value> Values {get; set;}
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<PercentageBreakdown> PercentageBreakdowns { get; set; }
    }
}