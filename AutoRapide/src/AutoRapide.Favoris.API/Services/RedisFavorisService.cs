using AutoRapide.Favoris.API.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace AutoRapide.Favoris.API.Services
{
    public class RedisFavorisService : IFavorisService
    {
        private readonly IDistributedCache _cache;

        public RedisFavorisService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public void AjouterFavori(int idVehicule, string ip)
        {
            var listeFavoris = _cache.GetString(ip);

            if(string.IsNullOrEmpty(listeFavoris))
            {
                SetValuesToFavoris(ip, new List<int> { idVehicule });
                return;
            }

            var listeVehicules = listeFavoris.Split(',')
                .Select(int.Parse)
                .ToList();

            foreach(var id in listeVehicules)
            {
                if (id == idVehicule)
                    return;
            }

            listeVehicules.Add(idVehicule);
            SetValuesToFavoris(ip, listeVehicules);
        }

        public void EffacerFavori(int idVehicule, string ip)
        {
            var listeFavoris = _cache.GetString(ip);
            if (string.IsNullOrEmpty(listeFavoris))
            {
                throw new InvalidDataException("Le véhicule ne se trouve pas dans les favoris.");
            }

            var listeVehicules = listeFavoris.Split(',')
                .Select(int.Parse)
                .ToList();
             
            if (!listeVehicules.Contains(idVehicule))
            {
                throw new InvalidDataException("Le véhicule ne se trouve pas dans les favoris.");
            }

            listeVehicules.Remove(idVehicule);
            SetValuesToFavoris(ip, listeVehicules);
        }

        public IEnumerable<int> ObtenirLesFavoris(string ip)
        {
            var listeFavoris = _cache.GetString(ip);
            if (string.IsNullOrEmpty(listeFavoris))
            {
                return new List<int>();
            }

            var listeVehicules = listeFavoris.Split(',').Select(int.Parse).ToList();
            return listeVehicules;
        }

        private void SetValuesToFavoris(string ip, List<int> data)
        {
            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            };
            _cache.SetString(ip, string.Join(',', data), cacheEntryOptions);
        }
    }
}
