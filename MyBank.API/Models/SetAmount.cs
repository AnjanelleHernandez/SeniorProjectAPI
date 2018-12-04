namespace MyBank.API.Models
{
    public class SetAmount
    {
        public int SetAmountID { get; set; }
        public string SetAmountName { get; set; }
        public Account AccountAssociated { get; set; }
        public int accountID { get; set; }
    }
}