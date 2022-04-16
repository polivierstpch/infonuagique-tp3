using AutoRapide.MVC.Interfaces;
using Newtonsoft.Json;

namespace AutoRapide.MVC.Services
{
    public class FichiersServicesProxy : IFichiersService
    {
        private const string RouteApi = "/api/";
        private readonly HttpClient _httpClient;
        private readonly ILogger<FichiersServicesProxy> _logger;

        public FichiersServicesProxy(HttpClient httpClient, ILogger<FichiersServicesProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        
        public async Task<IEnumerable<string>> EnvoyerFichiers(string codeVehicule, IEnumerable<IFormFile> fichiers)
        {
            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(codeVehicule), "codeVehicule");

            foreach (var fichier in fichiers)
            {
                formData.Add(new StreamContent(fichier.OpenReadStream()), "fichiers", fichier.FileName);
            }
            var reponse = await _httpClient.PostAsync(RouteApi + "upload", formData);

            if (reponse.IsSuccessStatusCode)
            {
                var content = await reponse.Content.ReadAsStringAsync();
                var nomsFichiers = JsonConvert.DeserializeObject<List<string>>(content);
                _logger.LogInformation(
                    "{Fichiers} ont été enregistrés dans le service de fichier avec succès (StatusCode: {StatusCode})",
                    string.Join(", ", nomsFichiers),
                    (int)reponse.StatusCode
                );
                return nomsFichiers;
            }
            
            _logger.LogError(
                "Les fichiers n'ont pas pu être enregistrés dans le service de fichier (StatusCode: {StatusCode}\nRaison: {Raison}",
                (int)reponse.StatusCode,
                reponse.ReasonPhrase
            );
            
            return new List<string>();
        }
    }
}
