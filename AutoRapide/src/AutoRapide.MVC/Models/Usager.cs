using System.ComponentModel.DataAnnotations;

namespace AutoRapide.MVC.Models
{
    public class Usager : BaseEntity
    {
        [Required]
        public string Nom { get; set; } = "";
        [Required]
        public string Prenom { get; set; } = "";
        [Required]
        public string Email { get; set; } = "";
        public string? Adresse { get; set; }
        public string? CodeUniqueUsager { get; set; }
    }
}
