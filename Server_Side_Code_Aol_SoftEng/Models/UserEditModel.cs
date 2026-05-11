using System.ComponentModel.DataAnnotations;

namespace Server_Side_Code_Aol_SoftEng.Models
{
    public class UserEditModel
    {
        [Required(ErrorMessage = "Username wajib diisi.")]
        public string Username { get; set; }
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Role wajib dipilih.")]
        public string Role { get; set; }
    }
}
