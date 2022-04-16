using System.ComponentModel.DataAnnotations;

namespace AutoRapide.Utilisateurs.API.Entities;

public abstract class BaseEntity
{
    [Key] 
    public virtual int Id { get; set; }
}