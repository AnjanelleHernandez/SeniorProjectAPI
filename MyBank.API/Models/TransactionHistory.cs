using System;

namespace MyBank.API.Models
{
    public class TransactionHistory
    {
        public int transactionHistoryID { get; set; }
        public DateTime transactionDateTime { get; set; }  
        public string transactionType { get; set; }
        public string TransactionDetail { get; set; }
        public Account accounts { get; set; }
        public int accountID { get; set; }
    }
}