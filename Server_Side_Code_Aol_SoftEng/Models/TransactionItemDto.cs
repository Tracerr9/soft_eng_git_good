namespace Server_Side_Code_Aol_SoftEng.Models
{
    public class TransactionItemDto
    {
        public string ProductSKU { get; set; }
        public decimal Price { get; set; }
        public int ProductAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string ProductName { get; set; }
    }
}
