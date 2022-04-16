using System.Net;
using Azure.Storage.Blobs;
using FichiersFunction;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutoRapide.Function
{
    public class FichiersBlobStorage
    {
        private readonly ILogger _logger;
        private readonly BlobContainerClient _containerClient;

        public FichiersBlobStorage(ILoggerFactory loggerFactory, IConfiguration config)
        {
            _logger = loggerFactory.CreateLogger<FichiersBlobStorage>();
            var blobClient = new BlobServiceClient(config.GetValue<string>("AzureWebJobsStorage"));
            _containerClient = blobClient.GetBlobContainerClient(config.GetValue<string>("ContainerName"));
        }  

        [Function("UploadToBlobStorage")]
        public async Task<HttpResponseData> UploadFiles([HttpTrigger(AuthorizationLevel.Anonymous, "post", "get", Route = "upload")] HttpRequestData request)
        {
            _logger.LogInformation("Method : {Method}", request.Method);
            if (request.Method == HttpMethods.Get)
            {
                var response = request.CreateResponse(HttpStatusCode.OK);
                response.WriteString("Fonction appelée.", System.Text.Encoding.UTF8);
                return response;
            }

            _logger.LogInformation("Commencement du chargement des fichiers...");

            var formData = await MultipartFormDataParser.ParseAsync(request.Body);
            var files = formData.Files;
            var codeVehicule = formData.GetParameterValue("codeVehicule");

            if (files.Count != 2 && (string.IsNullOrEmpty(codeVehicule) || codeVehicule.Length != 17))
            {
                _logger.LogError(
                    "Le contenu de la requête n'est pas valide.\ncodeVehicule = {CodeVehicule}\n nombre de fichiers = {NbFichiers}",
                    codeVehicule,
                    files.Count
                );

                var response = request.CreateResponse(HttpStatusCode.BadRequest);
                response.WriteString("Le contenu de la requete est invalide.");
                return response;
            }

            try
            {
                var nomsFichiers = new List<string>();
                var tachesUpload = new List<Task>();

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var nomFichier = $"{codeVehicule}_{i + 1}{Path.GetExtension(file.FileName)}";

                    _logger.LogInformation("Enregistrement du fichier {Fichier}.", nomFichier);

                    nomsFichiers.Add(nomFichier);
                    tachesUpload.Add(_containerClient.UploadBlobAsync(nomFichier, file.Data));
                }

                await Task.WhenAll(tachesUpload);

                _logger.LogInformation(
                    "Les fichiers {Fichier1} et {Fichier2} ont été enregistrés avec succès.",
                    nomsFichiers[0],
                    nomsFichiers[1]
                );

                var response = request.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync<List<string>>(nomsFichiers);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Une erreur est survenue lors de l'enregistrement des fichiers :\n{Exception}", ex.Message);

                var response = request.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync(new { Erreur = "Une erreur est survenue lors de l'enregistrement des fichiers." });

                return response;
            }
        }
    }
}
