namespace MyBank.API.Dtos
{
    public class RemoveFundsFromAccountDto
    {
        public int accountID { get; set; }
        public decimal amountToBeRemoved { get; set; }
        public int percentageID { get; set; }
        public string transactionType { get; set; }
        public string transactionDetail { get; set; }
    }
}