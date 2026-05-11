using System.ComponentModel.DataAnnotations;

namespace Server_Side_Code_Aol_SoftEng.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Username wajib diisi.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password wajib diisi.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role wajib dipilih.")]
        public string Role { get; set; }
    }
}
