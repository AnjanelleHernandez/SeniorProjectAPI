using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MyBank.API.Models
{
    public class Account
    {
        public int accountID { get; set; }
        public string accountName { get; set; }
        public decimal accountTotal { get; set; }

        //foreign key to User Table
        public User User { get; set; }
        public int userID { get; set; }

        public ICollection<TransactionHistory> TransactionHistory { get; set; }
    }
}