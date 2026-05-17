namespace Server_Side_Code_Aol_SoftEng.Models
{
    public class ProductNotificationDto
    {
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public int Stock { get; set; }
        public int StockThreshold { get; set; }
    }
}
