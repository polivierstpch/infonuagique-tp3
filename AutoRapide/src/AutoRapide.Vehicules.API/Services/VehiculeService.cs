using System.Linq.Expressions;
using AutoRapide.Vehicules.API.Entities;
using AutoRapide.Vehicules.API.Interfaces;

namespace AutoRapide.Vehicules.API.Services;

public class VehiculeService : IVehiculeService
{
    private readonly IAsyncRepository<Vehicule> _repository;

    public VehiculeService(IAsyncRepository<Vehicule> repository)
    {
        _repository = repository;
    }
    
    public async Task EnregistrerAsync(Vehicule vehicule)
    {
        await _repository.AjouterAsync(vehicule);
    }

    public async Task<Vehicule> TrouverParIdAsync(int id)
    {
        return await _repository.ObtenirParIdAsync(id);
    }

    public async Task SupprimerAsync(Vehicule vehicule)
    {
        await _repository.SupprimerAsync(vehicule);
    }

    public async Task ModifierAsync(Vehicule vehicule)
    {
        await _repository.ModifierAsync(vehicule);
    }
    
    public async Task<IEnumerable<Vehicule>> ObtenirListe()
    {
        return await _repository.ObtenirToutAsync();
    }

    public async Task<IEnumerable<Vehicule>> ChercherListe(Expression<Func<Vehicule, bool>> predicat)
    {
        return await _repository.ObtenirListeAsync(predicat);
    }
}