using AutoRapide.Utilisateurs.API.Entities;
using AutoRapide.Utilisateurs.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoRapide.Utilisateurs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsagerController : ControllerBase
    {
        private readonly IUsagerService _crudService;
        private readonly ILogger<UsagerController> _logger;

        public UsagerController(IUsagerService crudService, ILogger<UsagerController> logger)
        {
            _crudService = crudService;
            _logger = logger;
        }

        /// <summary>
        /// Premet l'obtention et le retour d'une liste de tous les usagers du site AutoRapide
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Liste complète des usagers de la bibliothèque Lipajoli trouvée et retournée</response>
        /// <response code="400">Liste complète des usagers de la bibliothèque Lipajoli introuvable</response>
        // GET: api/Usagers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usager>>> Get()
        {

            var usagers = await _crudService.ObtenirTousLesUsagers();
            _logger.LogInformation(CustomLogEvents.Lecture, $"Obtention de {usagers.ToList().Count} usagers");

            if (usagers == null || usagers.Count() == 0)
            {
                _logger.LogError(CustomLogEvents.Lecture, $"Échec d'obtention des usagers.");
                return BadRequest();
            }
            else
            {
                return Ok(usagers);
            }  
        }

        /// <summary>
        /// Premet l'obtention et le retour des informations d'un usager spécifique, ciblé par l'id passé en paramètre
        /// </summary>
        /// <param name="code">Code de l'utilisateur</param>
        /// <returns></returns>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">L'usager spécifié a été trouvé et retourné</response>
        /// <response code="400">Usager introuvable pour l'id specifié</response>
        [HttpGet("{code}")]
        public async Task<ActionResult<Usager>> Get(string code)
        {
            var usager = await _crudService.ObtenirUsagerParCodeUsager(code);
            _logger.LogInformation(CustomLogEvents.Lecture, $"Obtention de l'usager avec le code: {code}.");
            if (usager == null)
            {
                _logger.LogError(CustomLogEvents.Lecture, $"Échec de l'obtention de l'usager avec le code: {code}.");
                return BadRequest();
            }
            else
            {
                return Ok(usager);
            }
    
        }

        /// <summary>
        /// Premet la création d'un nouvel usager du site AutoRapide
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="201">L'usager a été créé avec succès!</response>
        /// <response code="400">L'usager n'a pas pu être enregistré. Veuillez vérifier la validité des informations.</response>
        [HttpPost]
        public async Task<ActionResult<Usager>> Post([FromBody] Usager usager)
        {
            try
            {
                await _crudService.AjouterUsager(usager);
                _logger.LogInformation(CustomLogEvents.Creation, $"Création d'un nouvel usager avec l'ID: {usager.Id}.");
                return CreatedAtAction(nameof(Post), new { id = usager.Id }, usager);
            } 
            catch(InvalidDataException ex)
            {
                _logger.LogError(CustomLogEvents.Creation, $"Échec de la création d'un nouvel usager: {ex.Message}");
                return BadRequest(ex.Message);
            }   
        }

        /// <summary>
        /// Premet la création d'un nouvel usager du site AutoRapide
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="201">L'usager a été créé avec succès!</response>
        /// <response code="400">L'usager n'a pas pu être modifié. Veuillez vérifier la validité des informations.</response>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Usager usager)
        {
            try
            {
                await _crudService.ModifierUsager(usager);
                _logger.LogInformation(CustomLogEvents.Modication, $"Modification de l'usager avec l'ID: {usager.Id}.");
                return new OkObjectResult(new { Message = $"L'usager avec l'id {usager.Id} a été modifié avec succès." });
            }
            catch (InvalidDataException ex)
            {
                _logger.LogError(CustomLogEvents.Modication, $"Échec de la modification de l'usager avec l'ID: {usager.Id}");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Premet la création d'un nouvel usager du site AutoRapide
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="201">L'usager a été créé avec succès!</response>
        /// <response code="400">L'usager à supprimer est inexistant.</response>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _crudService.EffacerUsager(id);
                _logger.LogInformation(CustomLogEvents.Suppression, $"Modification de l'usager avec l'ID: {id}.");
                return new OkObjectResult(new { Message = $"L'usager avec l'id {id} a été supprimé avec succès." });
            }
            catch (InvalidDataException ex)
            {
                _logger.LogError(CustomLogEvents.Modication, $"Échec de la suppression de l'usager avec l'ID: {id}");
                return BadRequest(ex.Message);
            }
        }
    }
}
