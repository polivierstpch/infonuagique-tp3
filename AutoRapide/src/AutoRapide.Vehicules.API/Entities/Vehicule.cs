using System.ComponentModel.DataAnnotations;

namespace AutoRapide.Vehicules.API.Entities;

public enum TypeVehicule
{
    Essence,
    Hybride
}

public class Vehicule
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Ce champ est requis.")]
    public string Constructeur { get; set; }
    
    [Required(ErrorMessage = "Ce champ est requis.")]
    public string Modele { get; set; }
    
    [Required(ErrorMessage = "Ce champ est requis.")]
    [Range(1900, 9999, ErrorMessage = "Veuillez fournir une date de fabrication valide (1900 à 9999).")]
    public int AnneeFabrication { get; set; }
    
    [Required(ErrorMessage = "Ce champ est requis.")]
    public TypeVehicule Type { get; set; }
    
    [Required(ErrorMessage = "Ce champ est requis.")]
    [Range(1, 99, ErrorMessage = "Veuillez fournir un nombre de siège(s) valide (1 à 99).")]
    public int NombreSiege { get; set; }
    
    [Required(ErrorMessage = "Ce champ est requis.")]
    public string Couleur { get; set; }
    
    [Required(ErrorMessage = "Ce champ est requis.")]
    [RegularExpression(@"^[A-Z0-9]{17}$", ErrorMessage = "Le NIV doit être composé de 17 caractères et seulement de caractères alphanumériques (en majuscule).")]
    public string NIV { get; set; }
    
    [Required(ErrorMessage = "Ce champ est requis.")]
    public string Image1Url { get; set; }

    public string Image2Url { get; set; }
    
    public string Description { get; set; }
    
    public bool EstDisponible { get; set; }
    
    [Required(ErrorMessage = "Ce champ est requis.")]
    [Range(1, 9_999_999.99, ErrorMessage = "Veuillez fournir un prix valide. (1,00 à 9,999,999.99)")]
    public double Prix { get; set; }
}