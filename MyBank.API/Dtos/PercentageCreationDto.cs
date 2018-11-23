namespace MyBank.API.Dtos
{
    public class PercentageCreationDto
    {
        public string percentageName { get; set; }
        public decimal percentageAmount { get; set; }
        public int accountID { get; set; }
    }
}