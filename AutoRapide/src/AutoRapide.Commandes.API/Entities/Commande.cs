using System.ComponentModel.DataAnnotations;

namespace AutoRapide.Commandes.API.Entities;

public class Commande
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int VehiculeId { get; set; }
    
    [Required]
    public int UsagerId { get; set; }
    
    [Required]
    public DateTime Date { get; set; } = DateTime.Now;
}