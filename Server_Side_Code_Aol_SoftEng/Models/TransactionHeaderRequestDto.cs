using System.ComponentModel.DataAnnotations;

namespace Server_Side_Code_Aol_SoftEng.Models
{
    public class TransactionHeaderRequestDto
    {
        [Required(ErrorMessage = "Tanggal transaksi wajib diisi.")]
        public DateTime? TransactionDate { get; set; }
        [Required(ErrorMessage = "Detail transaksi wajib diisi.")]
        [MinLength(1, ErrorMessage = "Minimal harus ada 1 detail transaksi.")]
        public List<TransactionDetailDto> Details { get; set; }
    }
}
