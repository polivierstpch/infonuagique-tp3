using AutoRapide.Utilisateurs.API.Entities;

namespace AutoRapide.Utilisateurs.API.Interfaces
{
    public interface IUsagerService
    {
        Task<Usager> ObtenirUsagerParId(int id);
        Task<Usager> ObtenirUsagerParCodeUsager(string code);
        Task<IEnumerable<Usager>> ObtenirTousLesUsagers();
        Task AjouterUsager(Usager usager);
        Task ModifierUsager(Usager usager);
        Task EffacerUsager(int id);
    }
}
