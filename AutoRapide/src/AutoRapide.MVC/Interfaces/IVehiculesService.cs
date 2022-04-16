using AutoRapide.MVC.Models;

namespace AutoRapide.MVC.Interfaces
{
    public interface IVehiculesService
    {
        public Task<IEnumerable<Vehicule>> ObtenirToutAsync();
        public Task<Vehicule> ObtenirParIdAsync(int id);
        public Task<HttpResponseMessage> AjouterAsync(Vehicule vehicule);
        public Task<HttpResponseMessage> ModifierAsync(Vehicule vehicule);
        public Task<HttpResponseMessage> SupprimerAsync(int id);
    }
}
