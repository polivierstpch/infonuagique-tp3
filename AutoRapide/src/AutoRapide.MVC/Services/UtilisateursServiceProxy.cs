using AutoRapide.MVC.Models;
using Newtonsoft.Json;
using System.Text;
using AutoRapide.MVC.Interfaces;

namespace AutoRapide.MVC.Services
{
    public class UtilisateursServiceProxy : IUsagerService
    {
        private const string _usagerApiUrl = "api/usager/";
        private readonly HttpClient _httpClient;
        private readonly ILogger<UtilisateursServiceProxy> _logger;
        
        public UtilisateursServiceProxy(HttpClient httpClient, ILogger<UtilisateursServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        
        public async Task<Usager> ObtenirUsagerParCodeUsager(string code)
        {
            var reponse = await _httpClient.GetAsync(_usagerApiUrl + code);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Usager (code: {Code}) obtenu avec succès (StatusCode: {StatusCode})",
                    code,
                    (int)reponse.StatusCode
                );
                var content = await reponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Usager>(content);
            }
            
            _logger.LogError(
                "L'usager (code: {Code}) n'a pas pu être obtenu (StatusCode: {StatusCode})\nRaison: {Raison}",
                code,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return null;
        }
        public async Task<IEnumerable<Usager>> ObtenirTousLesUsagers()
        {
            var reponse = await _httpClient.GetAsync(_usagerApiUrl);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Tous les usagers ont été obtenus avec succès (StatusCode: {StatusCode})",
                    (int)reponse.StatusCode
                );
                var content = await reponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Usager>>(content);
            }
            
            _logger.LogError(
                "Les usagers n'ont pas pu être obtenus (StatusCode: {StatusCode})\nRaison: {Raison}",
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );

            return new List<Usager>();
        }
        public async Task<HttpResponseMessage> AjouterUsager(Usager usager) {
            var content = new StringContent(JsonConvert.SerializeObject(usager), Encoding.UTF8, "application/json");
            var reponse = await _httpClient.PostAsync(_usagerApiUrl, content);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "l'usager a été ajouté avec succès (StatusCode: {StatusCode})",
                    (int)reponse.StatusCode
                );
                return reponse;
            }
            
            _logger.LogError(
                "L'usager n'a pas pu être ajouté (StatusCode: {StatusCode})\nRaison: {Raison}",
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );

            return reponse;
        }
        public async Task<HttpResponseMessage> ModifierUsager(Usager usager) {
            var content = new StringContent(JsonConvert.SerializeObject(usager), Encoding.UTF8, "application/json");
            var reponse = await _httpClient.PutAsync(_usagerApiUrl + usager.CodeUniqueUsager, content);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "l'usager (id: {Id}) a été modifié avec succès (StatusCode: {StatusCode})",
                    usager.Id,
                    (int)reponse.StatusCode
                );
                return reponse;
            }
            
            _logger.LogError(
                "L'usager (id: {Id}) n'a pas pu être modifié (StatusCode: {StatusCode})\nRaison: {Raison}",
                usager.Id,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );

            return reponse;
        }
        public async Task<HttpResponseMessage> EffacerUsager(string code) {
            var reponse = await _httpClient.DeleteAsync(_usagerApiUrl + code);
            if (reponse.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "l'usager (code: {Code}) a été supprimé avec succès (StatusCode: {StatusCode})",
                    code,
                    (int)reponse.StatusCode
                );
                return reponse;
            }
            
            _logger.LogError(
                "L'usager (code: {Code}) n'a pas pu être supprimé (StatusCode: {StatusCode})\nRaison: {Raison}",
                code,
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );

            return reponse;
        }
    }
}
