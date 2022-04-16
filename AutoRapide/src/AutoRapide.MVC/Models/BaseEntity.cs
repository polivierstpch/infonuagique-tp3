using System.ComponentModel.DataAnnotations;

namespace AutoRapide.MVC.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
