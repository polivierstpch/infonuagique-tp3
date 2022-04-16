using AutoRapide.Vehicules.API.Entities;
using AutoRapide.Vehicules.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoRapide.Vehicules.API.Controllers;

[Route("api/vehicules")]
[ApiController]
public class GestionVehiculeController : ControllerBase
{
    private readonly IVehiculeService _vehiculeService;
    private readonly ILogger<GestionVehiculeController> _logger;
    
    public GestionVehiculeController(IVehiculeService vehiculeService, ILogger<GestionVehiculeController> logger)
    {
        _vehiculeService = vehiculeService;
        _logger = logger;
    }
    
    /// <summary>
    /// Retourne tous les véhicules dans la base de données.
    /// </summary>
    /// <returns>Une réponse HTTP Ok avec une liste de véhicule dans le corps de celle-ci.</returns>
    /// <response code="200">Une liste de véhicule a été retournée.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vehicule>>> ObtenirTout()
    {
        var vehicules = await _vehiculeService.ObtenirListe();
        _logger.LogInformation(CustomLogEvents.Lecture, "Obtention de tous les véhicules de la base de données");
        return Ok(vehicules);
    }
    
    /// <summary>
    /// Effectue l'obtention d'un véhicule spécifique dans la base de données selon l'identifiant passé dans le corps de la requête.
    /// </summary>
    /// <param name="id">L'identifiant du véhicule à obtenir.</param>
    /// <returns>Une réponse HTTP Ok avec le véhicule demandé dans le corps de celle-ci.</returns>
    /// <response code="200">Le véhicule a été trouvé et a été envoyé dans la réponse.</response>
    /// <response code="404">Le véhicule avec l'identifiant n'a pas été trouvé dans la base de données.</response>
    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Vehicule>> ObtenirUn(int id)
    {
        var vehicule = await _vehiculeService.TrouverParIdAsync(id);

        if (vehicule is null)
        {
            _logger.LogInformation(CustomLogEvents.Lecture,"Le véhicule avec l'identifiant {Id}, n'a pas été trouvé", id);
            return NotFound();
        }
        
        _logger.LogInformation(CustomLogEvents.Lecture,"Le véhicule avec l'identifiant {Id} a été trouvé", id);
        return Ok(vehicule);
    }
    
    /// <summary>
    /// Effectue l'ajout d'un véhicule dans la base de données.
    /// </summary>
    /// <param name="vehicule">Le véhicule à ajouter dans la base de données.</param>
    /// <returns>Une réponse HTTP Created, avec le lien de l'API pour y accéder par l'identifiant.</returns>
    /// <response code="201">Le véhicule a bien été créé dans la base de données.</response>
    /// <response code="400">Le véhicule fourni n'est pas valide.</response>
    [HttpPost]
    [Route("enregistrer")]
    public async Task<IActionResult> Ajouter([FromBody] Vehicule vehicule)
    {
        await _vehiculeService.EnregistrerAsync(vehicule);
        _logger.LogInformation(CustomLogEvents.Creation, "Création du véhicule réussi, avec l'identifiant {Id}", vehicule.Id);
        return CreatedAtAction(nameof(ObtenirUn), new {id = vehicule.Id}, vehicule);

    }

    /// <summary>
    /// Effectue la suppression d'un véhicule dans la base de données selon l'identifiant passé en paramètre.
    /// </summary>
    /// <param name="id">Identifiant du véhicule dans la base de données.</param>
    /// <returns>Une réponse HTTP NoContent, si la suppression a réussi.</returns>
    /// <response code="204">L'action de supprimer à bien été reçue.</response>
    /// <response code="404">Le véhicule avec l'identifiant n'a pas été trouvé dans la base de données.</response>
    /// <response code="500">Une erreur est survenue lors de la suppression du véhicule dans la base de données.</response>
    [HttpDelete]
    [Route("supprimer/{id:int}")]
    public async Task<IActionResult> Supprimer(int id)
    {
        var vehiculeASupprimer = await _vehiculeService.TrouverParIdAsync(id);

        if (vehiculeASupprimer is null)
        {
            _logger.LogInformation(CustomLogEvents.Suppression, "Le véhicule avec l'identifiant {Id} n'a pas été trouvé", id);
            return NotFound();
        }
        
        await _vehiculeService.SupprimerAsync(vehiculeASupprimer);
        _logger.LogInformation(CustomLogEvents.Suppression, "Le véhicule avec l'identifiant {Id} a été supprimé", id);

        return NoContent();
    }
    
    
    /// <summary>
    /// Effectue la modification d'un véhicule dans la base de données selon les valeurs passées dans le corps de la requête.
    /// </summary>
    /// <remarks>Aucune remarque.</remarks>
    /// <param name="id">Identifiant du véhicule dans la base de données.</param>
    /// <param name="vehicule">Véhicule à modifier.</param>
    /// <returns>Une réponse HTTP NoContent, si la modification a réussi.</returns>
    /// <response code="204">L'action de modifier à bien été reçue.</response>
    /// <response code="400">L'identifiant fourni n'est pas valide selon le véhicule passé.</response>
    /// <response code="400">Le véhicule fourni n'est pas valide.</response>
    /// <response code="404">Le véhicule avec l'identifiant n'a pas été trouvé dans la base de données.</response>
    /// <response code="500">Une erreur est survenue lors de la modification du véhicule dans la base de données.</response>
    [HttpPut]
    [Route("modifier/{id:int}")]
    public async Task<IActionResult> Modifier(int id, [FromBody] Vehicule vehicule)
    {
        if (id != vehicule.Id)
        {
            _logger.LogInformation(CustomLogEvents.Modification, 
                "L'identifiant {Id} ne correspond pas a l'identifiant du véhicule à modifier ({IdVehicule})",
                id,
                vehicule.Id);
            return BadRequest("L'identifant n'est pas le même que celui du véhicule du corps de la requête.");
        }

        var vehiculeExistant = await _vehiculeService.TrouverParIdAsync(id);

        if (vehiculeExistant is null)
        {
            _logger.LogInformation(CustomLogEvents.Modification,
                "Le véhicule avec l'identifiant {Id} n'existe pas dans la base de données",
                id);
            return NotFound();
        }

        await _vehiculeService.ModifierAsync(vehicule);
        _logger.LogInformation(CustomLogEvents.Modification, "Le véhicule avec l'identifiant {Id} a bien été modifié", id);

        return NoContent();
    }
}