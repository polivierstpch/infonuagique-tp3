using AutoRapide.Utilisateurs.API.Entities;
using System.Linq.Expressions;

namespace AutoRapide.Utilisateurs.API.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> ObtenirParIdAsync(int id);
        Task<IEnumerable<T>> ObtenirToutAsync();
        Task<IEnumerable<T>> ObtenirListeAsync(Expression<Func<T, bool>> predicat);
        Task AjouterAsync(T entite);
        Task SupprimerAsync(T entite);
        Task ModifierAsync(T entite);
    }
}