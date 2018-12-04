namespace MyBank.API.Dtos
{
    public class SetAmountCreationDto
    {
        public string SetAmountName { get; set; }
        public decimal Total { get; set; }
        public int accountID { get; set; }
    }
}