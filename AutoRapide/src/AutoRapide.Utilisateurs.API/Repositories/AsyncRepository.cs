using AutoRapide.Utilisateurs.API.Data;
using AutoRapide.Utilisateurs.API.Entities;
using AutoRapide.Utilisateurs.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutoRapide.Utilisateurs.API.Repositories
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly UsagerContext _context;

        public AsyncRepository(UsagerContext context)
        {
            _context = context;
        }

        public async Task<T> ObtenirParIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> ObtenirToutAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> ObtenirListeAsync(Expression<Func<T, bool>> predicat)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(predicat)
                .ToListAsync();
        }

        public async Task AjouterAsync(T entite)
        {
            _context.Set<T>().Add(entite);
            await _context.SaveChangesAsync();
        }


        public async Task SupprimerAsync(T entite)
        {
            _context.Set<T>().Remove(entite);
            await _context.SaveChangesAsync();
        }

        public async Task ModifierAsync(T entite)
        {
            _context.Set<T>().Update(entite);
            await _context.SaveChangesAsync();
        }

    }
}