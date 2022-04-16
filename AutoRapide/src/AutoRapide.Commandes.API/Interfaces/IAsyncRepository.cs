using System.Linq.Expressions;

namespace AutoRapide.Commandes.API.Interfaces;

public interface IAsyncRepository<T>
{
    Task<T> ObtenirParIdAsync(int id);
    Task<IEnumerable<T>> ObtenirToutAsync();
    Task<IEnumerable<T>> ObtenirListeAsync(Expression<Func<T, bool>> predicat);
    Task AjouterAsync(T entite);
    Task ModifierAsync(T entite);
    Task SupprimerAsync(T entite);
}