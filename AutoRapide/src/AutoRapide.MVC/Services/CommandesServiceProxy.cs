using System.Text;
using AutoRapide.MVC.Interfaces;
using AutoRapide.MVC.Models;
using Newtonsoft.Json;

namespace AutoRapide.MVC.Services
{
    public class CommandesServiceProxy : ICommandesService
    {
        private const string RouteApi = "/api/commandes/";
        private readonly HttpClient _httpClient;
        private readonly ILogger<CommandesServiceProxy> _logger;
        
        public CommandesServiceProxy(HttpClient httpClient, ILogger<CommandesServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Commande> ObtenirParIdAsync(int id)
        {
            var reponse = await _httpClient.GetAsync($"{RouteApi}{id}");
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "La commande (id: {Id}) a été récupéré avec succès (StatusCode: {StatusCode})",
                    id,
                    (int)reponse.StatusCode
                );
                var content = await reponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Commande>(content);
            }
            
            _logger.LogError(
                "La commande (id: {Id}) n'a pas pu être récupéré (StatusCode: {StatusCode})\nRaison : {Raison}",
                id,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return null;
        }

        public async Task<IEnumerable<Commande>> ObtenirToutAsync()
        {
            var reponse = await _httpClient.GetAsync($"{RouteApi}");
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Toutes les commandes ont été récupérées avec succès (StatusCode: {StatusCode})",
                    (int)reponse.StatusCode
                );
                var content = await reponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Commande>>(content);
            }
            
            _logger.LogError(
                "Les commandes n'ont pas pu être récupérées (StatusCode: {StatusCode})\nRaison : {Raison}",
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return new List<Commande>();
        }

        public async Task<IEnumerable<Commande>> ObtenirToutPourUsagerAsync(int idUsager)
        { 
            var reponse = await _httpClient.GetAsync($"{RouteApi}usager/{idUsager}");
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Les commandes de l'usager (id: {Id}) ont été récupérées avec succès (StatusCode: {StatusCode})",
                    idUsager,
                    (int)reponse.StatusCode
                );
                var content = await reponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Commande>>(content);
            }
            
            _logger.LogError(
                "Les commandes de l'usager (id: {Id} n'ont pas pu être récupérées (StatusCode: {StatusCode})\nRaison : {Raison}",
                idUsager,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return new List<Commande>();
        }

        public async Task<HttpResponseMessage> AjouterAsync(Commande commande)
        {
            var content = new StringContent(JsonConvert.SerializeObject(commande), Encoding.UTF8, "application/json");
            
            var reponse = await _httpClient.PostAsync($"{RouteApi}enregistrer", content);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "La commande a été ajoutée avec succès (StatusCode: {StatusCode})", 
                    (int)reponse.StatusCode
                );
                return reponse;
            }
            
            _logger.LogError(
                "L'ajout de la commande a échoué. (StatusCode: {StatusCode})\nRaison : {Raison}",
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return reponse;
        }

        public async Task<HttpResponseMessage> ModifierAsync(Commande commande)
        {
            var content = new StringContent(JsonConvert.SerializeObject(commande), Encoding.UTF8, "application/json");
            
            var reponse = await _httpClient.PutAsync($"{RouteApi}modifier/{commande.Id}", content);

            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "La commande (id: {Id}) a été modifiée avec succès (StatusCode: {StatusCode})", 
                    commande.Id,
                    (int)reponse.StatusCode
                );
                return reponse;
            }

            _logger.LogError(
                "La modification de la commande (id: {Id}) a échoué. (StatusCode: {StatusCode})\nRaison : {Raison}",
                commande.Id,
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
                    "La commande (id: {Id}) a été supprimée avec succès (StatusCode: {StatusCode})", 
                    id,
                    (int)reponse.StatusCode
                );
                return reponse;
            }

            _logger.LogError(
                "La suppression de la commande (id: {Id}) a échoué. (StatusCode: {StatusCode})\nRaison : {Raison}",
                id,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return reponse;
        }
    }
}
