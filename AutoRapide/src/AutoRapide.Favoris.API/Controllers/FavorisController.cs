using AutoRapide.Favoris.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoRapide.Favoris.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavorisController : ControllerBase
    {
        private readonly IFavorisService _crudService;
        private readonly ILogger<FavorisController> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public FavorisController(IFavorisService crudService, ILogger<FavorisController> logger, IHttpContextAccessor httpContext)
        {
            _crudService = crudService;
            _logger = logger;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Premet l'obtention et le retour d'une liste de tous les véhicules dans les favoris en cache de l'utilisateur
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Liste complète des usagers de la bibliothèque Lipajoli trouvée et retournée</response>
        // GET: api/Usagers
        [HttpGet]
        public ActionResult<IEnumerable<int>> Get()
        {
            var ip = _httpContext.HttpContext.Connection.RemoteIpAddress?.ToString();
            var favoris = _crudService.ObtenirLesFavoris(ip);
            _logger.LogInformation(CustomLogEvents.Lecture, $"Obtention de {favoris.ToList().Count} favoris en cache.");
            return Ok(favoris);

        }

        /// <summary>
        /// Ajout d'un véhicule favoris dans la cache de l'utilisateur
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="201">Le véhicule a été ajouté aux favoris avec succès!</response>
        [HttpPost]
        public ActionResult<int> Post([FromBody]int idVehicule)
        {
            var ip = _httpContext.HttpContext.Connection.RemoteIpAddress?.ToString();
            _crudService.AjouterFavori(idVehicule, ip);
            _logger.LogInformation(CustomLogEvents.Creation, $"Ajout du véhicule avec l'ID: {idVehicule} aux favoris.");
            return new OkObjectResult(new { Message = $"Le véhicule avec l'id {idVehicule} a été ajouté aux favoris avec succès." });
        }

        /// <summary>
        /// Retirer un véhicule des favoris en cache de l'utilisateur
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="201">L'usager a été créé avec succès!</response>
        /// <response code="400">L'usager à supprimer est inexistant.</response>
        [HttpDelete("{idVehicule:int}")]
        public ActionResult Delete(int idVehicule)
        {
            try
            {
                var ip = _httpContext.HttpContext.Connection.RemoteIpAddress?.ToString();
                _crudService.EffacerFavori(idVehicule, ip);
                _logger.LogInformation(CustomLogEvents.Suppression, $"Supression du véhicule avec l'ID: {idVehicule} des favoris.");
                return new OkObjectResult(new { Message = $"Le véhicule avec l'id {idVehicule} a été retiré aux favoris avec succès." });
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(CustomLogEvents.Suppression, $"Échec de la suppression du véhicule avec l'ID: {idVehicule} des favoris");
                return BadRequest(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                _logger.LogError(CustomLogEvents.Suppression, $"Échec de la suppression du véhicule avec l'ID: {idVehicule} des favoris");
                return BadRequest(ex.Message);
            }
        }
    }
}
