using AutoRapide.MVC.Models;

namespace AutoRapide.MVC.Interfaces
{
    public interface IUsagerService
    {
        Task<Usager> ObtenirUsagerParCodeUsager(string code);
        Task<IEnumerable<Usager>> ObtenirTousLesUsagers();
        Task<HttpResponseMessage> AjouterUsager(Usager usager);
        Task<HttpResponseMessage> ModifierUsager(Usager usager);
        Task<HttpResponseMessage> EffacerUsager(string code);
    }
}
