using System.ComponentModel.DataAnnotations;

namespace ArabaKiralama.Models
{
    public class Car
    {
        [Key]
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

        [Display(Name = "Günlük Fiyat")]
        public decimal PricePerDay { get; set; }

        // YENİ EKLENEN ALAN: Fotoğraf URL'si
        [Display(Name = "Fotoğraf Linki (URL)")]
        public string ImageUrl { get; set; }
    }
}