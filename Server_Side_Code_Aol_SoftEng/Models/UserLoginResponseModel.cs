namespace Server_Side_Code_Aol_SoftEng.Models
{
    public class UserLoginResponseModel
    {
        public string? Token { get; set; }
        public string Message { get; set; }
        public DateTime? ExpireTime { get; set; }
        public string? Username { get; set; }
    }
}
