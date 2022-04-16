using System.ComponentModel.DataAnnotations;

namespace AutoRapide.MVC.Models;

public class Commande : BaseEntity
{
    [Required]
    public int VehiculeId { get; set; }
    
    [Required]
    public int UsagerId { get; set; }
    
    public DateTime Date { get; set; } = DateTime.Now;
}