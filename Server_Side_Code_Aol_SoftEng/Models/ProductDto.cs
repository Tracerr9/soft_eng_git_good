using System.ComponentModel.DataAnnotations;

namespace Server_Side_Code_Aol_SoftEng.Models
{
    public class ProductDto
    {
        [Required(ErrorMessage = "SKU Wajib diisi atau dikirim.")]
        public string SKU { get; set; }
        [Required(ErrorMessage = "Nama produk wajib diisi.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Jumlah stock wajib diisi.")]
        public int? Stock { get; set; }
        [Required(ErrorMessage = "Ambang batas stock wajib diisi.")]
        public int? StockThreshold { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Diskon tidak boleh kurang dari 0.")]
        public decimal Discount { get; set; }
        [Required(ErrorMessage = "Harga awal wajib diisi.")]
        [Range(0, double.MaxValue, ErrorMessage = "Harga tidak boleh kurang dari 0.")]
        public decimal? BasePrice { get; set; }
    }
}
