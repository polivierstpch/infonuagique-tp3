using System.Linq.Expressions;
using AutoRapide.Vehicules.API.Entities;

namespace AutoRapide.Vehicules.API.Interfaces;

public interface IVehiculeService
{
    Task EnregistrerAsync(Vehicule vehicule);
    Task<Vehicule> TrouverParIdAsync(int id);
    Task SupprimerAsync(Vehicule vehicule);
    Task ModifierAsync(Vehicule vehicule);
    Task<IEnumerable<Vehicule>> ObtenirListe();
    Task<IEnumerable<Vehicule>> ChercherListe(Expression<Func<Vehicule, bool>> predicat);
}