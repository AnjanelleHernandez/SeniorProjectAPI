using System.Collections.Generic;

namespace MyBank.API.Models
{
    public class PercentageBreakdown
    {
        public int PercentageBreakdownID { get; set; }
        public string PercentageBreakdownName { get; set; }
    //percentage
        public decimal PercentageAmount { get; set; }
    //amount of money from the percentage
        public decimal PercentageTotal { get; set; }
        public Account AccountAssociated { get; set; }
        public int accountID { get; set; }

        public ICollection<Account> Account { get; set; }
    }
}