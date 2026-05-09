using System.ComponentModel.DataAnnotations;

namespace Server_Side_Code_Aol_SoftEng.Models
{
    public class UserLoginRequestModel
    {
        [Required(ErrorMessage = "Username wajib diisi.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password wajib diisi.")]
        public string Password { get; set; }
    }
}
