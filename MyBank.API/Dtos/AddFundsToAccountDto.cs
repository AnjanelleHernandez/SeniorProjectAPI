namespace MyBank.API.Dtos
{
    public class AddFundsToAccountDto
    {
        public int accountID { get; set; }
        public decimal amountToBeAdded { get; set; }
        public string transactionType { get; set; }
        public string transactionDetail { get; set; }
    }
}