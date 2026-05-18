namespace Server_Side_Code_Aol_SoftEng.Models
{
    public class TransactionResponseDto
    {
        public int TransactionId { get; set; }
        public decimal Total { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
    }
}
