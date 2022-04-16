using System.Linq.Expressions;
using AutoRapide.Commandes.API.Data;
using AutoRapide.Commandes.API.Entities;
using AutoRapide.Commandes.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoRapide.Commandes.API.Repositories;

public class CommandeRepository : IAsyncRepository<Commande>
{
    private readonly CommandeContext _contexte;
    
    public CommandeRepository(CommandeContext contexte)
    {
        _contexte = contexte;
    }
    
    public async Task<Commande> ObtenirParIdAsync(int id)
    {
        return await _contexte.Set<Commande>()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Commande>> ObtenirToutAsync()
    {
        return await _contexte.Set<Commande>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> ObtenirListeAsync(Expression<Func<Commande, bool>> predicat)
    {
        return await _contexte.Set<Commande>()
            .AsNoTracking()
            .Where(predicat)
            .ToListAsync();
    }

    public async Task AjouterAsync(Commande entite)
    {
        _contexte.Set<Commande>().Add(entite);
        await _contexte.SaveChangesAsync();
    }

    public async Task ModifierAsync(Commande entite)
    {
        _contexte.Set<Commande>().Update(entite);
        await _contexte.SaveChangesAsync();
    }

    public async Task SupprimerAsync(Commande entite)
    {
        _contexte.Set<Commande>().Remove(entite);
        await _contexte.SaveChangesAsync();
    }
}