using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArabaKiralama.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // PostgreSQL'e "Numarayı sen ver" der.
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka zorunludur")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model zorunludur")]
        public string Model { get; set; }

        public int Year { get; set; }
        public string Plate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerDay { get; set; }

        public string ImageUrl { get; set; }
    }
}