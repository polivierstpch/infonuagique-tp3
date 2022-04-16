using Newtonsoft.Json;
using System.Text;
using AutoRapide.MVC.Interfaces;

namespace AutoRapide.MVC.Services
{
    public class FavorisServiceProxy : IFavorisService
    {
        private const string _favorisApiUrl = "api/Favoris/";
        private readonly HttpClient _httpClient;
        private readonly ILogger<FavorisServiceProxy> _logger;
        public FavorisServiceProxy(HttpClient httpClient, ILogger<FavorisServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IEnumerable<int>> ObtenirLesFavoris() 
        {
            var reponse = await _httpClient.GetAsync(_favorisApiUrl);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Obtenus les favoris avec succès (StatusCode: {StatusCode})",
                    (int)reponse.StatusCode
                );
                var content = await reponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<int>>(content);
            }

            _logger.LogError(
                "Les favoris n'ont pas pu être obtenus (StatusCode: {StatusCode})\nRaison: {Raison}",
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return new List<int>();
        }
        public async Task<HttpResponseMessage> AjouterFavori(int idVehicule) 
        {
            var content = new StringContent(JsonConvert.SerializeObject(idVehicule), Encoding.UTF8, "application/json");
            var reponse = await _httpClient.PostAsync(_favorisApiUrl, content);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Ajout du véhicule (id: {Id}) dans les favoris avec succès (StatusCode: {StatusCode})",
                    idVehicule,
                    (int)reponse.StatusCode
                );
                return reponse;
            }

            _logger.LogError(
                "Le véhicule (id: {Id}) n'a pas pu être ajouté aux favoris (StatusCode: {StatusCode})\nRaison: {Raison}",
                idVehicule,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );

            return reponse;
        }
        public async Task<HttpResponseMessage> EffacerFavori(int idVehicule) 
        {
            var reponse = await _httpClient.DeleteAsync(_favorisApiUrl + idVehicule);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Suppression du véhicule (id: {Id}) des favoris avec succès (StatusCode: {StatusCode})",
                    idVehicule,
                    (int)reponse.StatusCode
                );
                return reponse;
            }

            _logger.LogError(
                "Le véhicule (id: {Id}) n'a pas pu être supprimé des favoris (StatusCode: {StatusCode})\nRaison: {Raison}",
                idVehicule,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );

            return reponse;
        }
    }
}
