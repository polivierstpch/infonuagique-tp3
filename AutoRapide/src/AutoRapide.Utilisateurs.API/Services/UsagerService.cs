using AutoRapide.Utilisateurs.API.Entities;
using AutoRapide.Utilisateurs.API.Interfaces;

namespace AutoRapide.Utilisateurs.API.Services
{
    public class UsagerService : IUsagerService
    {
        private readonly IAsyncRepository<Usager> _usagerRepository;
        public UsagerService(IAsyncRepository<Usager> usagerRepository)
        {
            _usagerRepository = usagerRepository;
        }
        public async Task<Usager> ObtenirUsagerParId(int id)
        {
            return await _usagerRepository.ObtenirParIdAsync(id);
        }
        public async Task<Usager> ObtenirUsagerParCodeUsager(string code)
        {
            var usager = (await _usagerRepository.ObtenirListeAsync(_ => _.CodeUniqueUsager == code))
                                                .FirstOrDefault();
            return usager;
 
        }
        public async Task<IEnumerable<Usager>> ObtenirTousLesUsagers()
        {
            return await _usagerRepository.ObtenirToutAsync();
        }

        public async Task AjouterUsager(Usager usager)
        {
            var usagerExistant = (await _usagerRepository.ObtenirListeAsync(_ => _.Email == usager.Email))
                                                .FirstOrDefault() != null;
            if (usagerExistant)
            {
                throw new InvalidDataException("Un usager avec ce courriel existe déjà.");
            }
            if(usager.Nom == null || usager.Email == null || usager.Prenom == null)
            {
                throw new InvalidDataException("Des informations sont manquantes. Veuillez fournir toutes les information obligatoires.");
            }
            else
            {
                usager.CodeUniqueUsager = Guid.NewGuid().ToString();
                _usagerRepository.AjouterAsync(usager);
            }
        }
        public async Task ModifierUsager(Usager usager) 
        {
            var usagerExistant = (await _usagerRepository.ObtenirListeAsync(_ => _.Id == usager.Id))
                                                .FirstOrDefault() != null;
            if (usagerExistant)
            {
                _usagerRepository.ModifierAsync(usager);
            }
            else
            {
                throw new InvalidDataException("L'usager à modifier est inexistant.");
            }
            if (usager.Nom == null || usager.Email == null || usager.Prenom == null)
            {
                throw new InvalidDataException("Des informations sont manquantes. Veuillez fournir toutes les information obligatoires.");
            }
        }
        public async Task EffacerUsager(int id)
        {
            var usagerExistant = (await _usagerRepository.ObtenirListeAsync(_ => _.Id == id))
                                                .FirstOrDefault();
            if (usagerExistant != null)
            {
                _usagerRepository.SupprimerAsync(usagerExistant);
            }
            else
            {
                throw new InvalidDataException("L'usager à effacer est inexistant.");
            }
        }
    }
}
