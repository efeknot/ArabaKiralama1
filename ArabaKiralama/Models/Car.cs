using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // 1. BU SATIR ŞART: Identity özelliği için

namespace ArabaKiralama.Models
{
    public class Car
    {
        [Key]
        // 2. KRİTİK DÜZELTME: PostgreSQL'e Id'yi otomatik artırmasını söyler
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka zorunludur")]
        [Display(Name = "Marka")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model zorunludur")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Display(Name = "Yıl")]
        public int Year { get; set; }

        [Display(Name = "Plaka")]
        public string Plate { get; set; }

        // Para birimleri için Column(TypeName = "decimal(18,2)") eklemek Render'da hata almanı önler
        [Display(Name = "Günlük Fiyat")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerDay { get; set; }

        [Display(Name = "Fotoğraf Linki (URL)")]
        public string ImageUrl { get; set; }
    }
}