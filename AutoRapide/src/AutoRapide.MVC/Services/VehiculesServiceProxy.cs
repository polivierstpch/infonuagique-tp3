using System.Text;
using AutoRapide.MVC.Interfaces;
using AutoRapide.MVC.Models;
using Newtonsoft.Json;

namespace AutoRapide.MVC.Services
{
    public class VehiculesServiceProxy : IVehiculesService
    {
        private const string RouteApi = "/api/vehicules/";
        private readonly HttpClient _httpClient;
        private readonly ILogger<VehiculesServiceProxy> _logger;

        public VehiculesServiceProxy(HttpClient httpClient, ILogger<VehiculesServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Vehicule>> ObtenirToutAsync()
        {
            var reponse = await _httpClient.GetAsync(RouteApi);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Tous les véhicules ont été récupérés avec succès (StatusCode: {StatusCode})",
                    (int)reponse.StatusCode
                );
                var content = await reponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Vehicule>>(content);
            }
            
            _logger.LogError(
                "Les véhicules n'ont pas pu être récupérés (StatusCode: {StatusCode}\nRaison: {Raison}",
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return new List<Vehicule>();
        }

        public async Task<Vehicule> ObtenirParIdAsync(int id)
        {
            var reponse = await _httpClient.GetAsync($"{RouteApi}{id}");
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Le véhicule (id: {Id}) a été récupéré avec succès (StatusCode: {StatusCode})",
                    id,
                    (int)reponse.StatusCode
                );
                var content = await reponse.Content.ReadAsStringAsync(); 
                return JsonConvert.DeserializeObject<Vehicule>(content);
            }  
            
            _logger.LogError(
                "Le véhicule (id: {Id}) n'a pas pu être récupéré (StatusCode: {StatusCode}\nRaison: {Raison}",
                id,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return null;
        }

        public async Task<HttpResponseMessage> AjouterAsync(Vehicule vehicule)
        {
            var content = new StringContent(JsonConvert.SerializeObject(vehicule), Encoding.UTF8, "application/json");
            var reponse = await _httpClient.PostAsync($"{RouteApi}enregistrer", content);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Le véhicule (NIV: {Niv}) a été ajouté avec succès (StatusCode: {StatusCode})",
                    vehicule.NIV,
                    (int)reponse.StatusCode
                );
                return reponse;
            }

            _logger.LogError(
                "Le véhicule (NIV: {Niv}) n'a pas pu être ajouté. (StatusCode: {StatusCode}\nRaison: {Raison}",
                vehicule.NIV,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return reponse;
        }

        public async Task<HttpResponseMessage> ModifierAsync(Vehicule vehicule)
        {
            var content = new StringContent(JsonConvert.SerializeObject(vehicule), Encoding.UTF8, "application/json");
            var reponse = await _httpClient.PutAsync($"{RouteApi}modifier/{vehicule.Id}", content);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Le véhicule (id: {Id}) a été modifié avec succès (StatusCode: {StatusCode})",
                    vehicule.Id,
                    (int)reponse.StatusCode
                );
                return reponse;
            }

            _logger.LogError(
                "Le véhicule (id: {Id}) n'a pas pu être ajouté (StatusCode: {StatusCode}\nRaison: {Raison}",
                vehicule.Id,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return reponse;
        }

        public async Task<HttpResponseMessage> SupprimerAsync(int id)
        {
            var reponse = await _httpClient.DeleteAsync($"{RouteApi}supprimer/{id}");
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Le véhicule (id: {Id}) a été supprimé avec succès (StatusCode: {StatusCode})",
                    id,
                    (int)reponse.StatusCode
                );
                return reponse;
            }

            _logger.LogError(
                "Le véhicule (id: {Id}) n'a pas pu être supprimé (StatusCode: {StatusCode}\nRaison: {Raison}",
                id,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return reponse;
        }
    }
}
