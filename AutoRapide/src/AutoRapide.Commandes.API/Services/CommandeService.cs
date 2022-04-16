using AutoRapide.Commandes.API.Entities;
using AutoRapide.Commandes.API.Interfaces;

namespace AutoRapide.Commandes.API.Services;

public class CommandeService : ICommandeService
{
    private readonly IAsyncRepository<Commande> _repository;

    public CommandeService(IAsyncRepository<Commande> repository)
    {
        _repository = repository;
    }
    
    public async Task<Commande> ObtenirParId(int id)
    {
        return await _repository.ObtenirParIdAsync(id);
    }

    public async Task<IEnumerable<Commande>> ObtenirTout()
    {
        return await _repository.ObtenirToutAsync();
    }

    public async Task<IEnumerable<Commande>> ObtenirCommandesParUsager(int idUsager)
    {
        return await _repository.ObtenirListeAsync(c => c.UsagerId == idUsager);
    }

    public async Task Enregistrer(Commande commande)
    {
        await _repository.AjouterAsync(commande);
    }

    public async Task Modifier(Commande commande)
    {
        await _repository.ModifierAsync(commande);
    }

    public async Task Supprimer(Commande commande)
    {
        await _repository.SupprimerAsync(commande);
    }

    public async Task<bool> CommandeAvecVehiculeExiste(int idVehicule)
    {
        var commandes = await _repository.ObtenirListeAsync(c => c.VehiculeId == idVehicule);
        return commandes.Any();
    }
}