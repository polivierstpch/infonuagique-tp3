using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoRapide.MVC.Models
{
    public enum TypeVehicule
    {
        Essence,
        Hybride
    }
    
    public class Vehicule : BaseEntity
    {
         
        [Required(ErrorMessage = "Ce champ est requis.")]
        public string Constructeur { get; set; }
        
        [DisplayName("Modèle")]
        [Required(ErrorMessage = "Ce champ est requis.")]
        public string Modele { get; set; }
        
        [DisplayName("Année")]
        [Required(ErrorMessage = "Ce champ est requis.")]
        [Range(1900, 9999, ErrorMessage = "Veuillez fournir une date de fabrication valide (1900 à 9999).")]
        public int AnneeFabrication { get; set; }
        
        [DisplayName("Carburant")]
        [Required(ErrorMessage = "Ce champ est requis.")]
        public TypeVehicule Type { get; set; }
    
        [DisplayName("Places")]
        [Required(ErrorMessage = "Ce champ est requis.")]
        [Range(1, 99, ErrorMessage = "Veuillez fournir un nombre de siège(s) valide (1 à 99).")]
        public int NombreSiege { get; set; }
    
        [Required(ErrorMessage = "Ce champ est requis.")]
        public string Couleur { get; set; }

        public string NIV { get; set; }
        
        public string Image1Url { get; set; }

        [Display(Name = "2")]
        public string Image2Url { get; set; }
    
        public string Description { get; set; }
    
        [Display(Name ="Disponibilité")]
        public bool EstDisponible { get; set; }
    
        [Required(ErrorMessage = "Ce champ est requis.")]
        [Range(1, 9_999_999.99, ErrorMessage = "Veuillez fournir un prix valide. (1,00 à 9,999,999.99)")]
        public double Prix { get; set; }
        
    }
}
