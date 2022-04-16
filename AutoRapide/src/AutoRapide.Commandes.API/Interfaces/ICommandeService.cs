using AutoRapide.Commandes.API.Entities;

namespace AutoRapide.Commandes.API.Interfaces;

public interface ICommandeService
{
    Task<Commande> ObtenirParId(int id);
    Task<IEnumerable<Commande>> ObtenirTout();
    Task<IEnumerable<Commande>> ObtenirCommandesParUsager(int idUsager);
    Task Enregistrer(Commande commande);
    Task Modifier(Commande commande);
    Task Supprimer(Commande commande);
    Task<bool> CommandeAvecVehiculeExiste(int idVehicule);

}