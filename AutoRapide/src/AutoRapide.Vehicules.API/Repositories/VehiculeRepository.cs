using System.Linq.Expressions;
using AutoRapide.Vehicules.API.Data;
using AutoRapide.Vehicules.API.Entities;
using AutoRapide.Vehicules.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoRapide.Vehicules.API.Repositories;

public class VehiculeRepository : IAsyncRepository<Vehicule>
{
    private readonly VehiculeContext _contexte;

    public VehiculeRepository(VehiculeContext contexte)
    {
        _contexte = contexte;
    }
    
    public async Task<Vehicule> ObtenirParIdAsync(int id)
    {
        return await _contexte
            .Set<Vehicule>()
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<Vehicule>> ObtenirToutAsync()
    {
        return await _contexte.Set<Vehicule>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicule>> ObtenirListeAsync(Expression<Func<Vehicule, bool>> predicat)
    {
        return await _contexte.Set<Vehicule>()
            .AsNoTracking()
            .Where(predicat)
            .ToListAsync();
    }

    public async Task AjouterAsync(Vehicule entite)
    {
        _contexte.Set<Vehicule>().Add(entite);
        await _contexte.SaveChangesAsync();
    }

    public async Task ModifierAsync(Vehicule entite)
    {
        _contexte.Set<Vehicule>().Update(entite);
        await _contexte.SaveChangesAsync();
    }

    public async Task SupprimerAsync(Vehicule entite)
    {
        _contexte.Set<Vehicule>().Remove(entite);
        await _contexte.SaveChangesAsync();
    }
    
}