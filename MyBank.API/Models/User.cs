using System.Collections.Generic;

namespace MyBank.API.Models
{
    public class User
    {
        public int ID { get; set; }
        public string firstName { get; set; }   
        public string lastName { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        public string emailAddress { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}