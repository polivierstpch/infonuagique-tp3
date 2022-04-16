using System.Net;
using AutoRapide.MVC.Models;

namespace AutoRapide.MVC.Interfaces
{
    public interface ICommandesService
    {
        Task<Commande> ObtenirParIdAsync(int id);
        Task<IEnumerable<Commande>> ObtenirToutAsync();
        Task<IEnumerable<Commande>> ObtenirToutPourUsagerAsync(int idUsager);
        Task<HttpResponseMessage> AjouterAsync(Commande commande);
        Task<HttpResponseMessage> ModifierAsync(Commande commande);
        Task<HttpResponseMessage> SupprimerAsync(int id);
    }
}
