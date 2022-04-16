
using AutoRapide.Commandes.API.Entities;
using AutoRapide.Commandes.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoRapide.Commandes.API.Controllers;

[Route("api/commandes")]
[ApiController]
public class GestionCommandesController : ControllerBase
{
    private readonly ICommandeService _commandeService;
    private readonly ILogger<GestionCommandesController> _logger;

    public GestionCommandesController(ICommandeService commandeService, ILogger<GestionCommandesController> logger)
    {
        _commandeService = commandeService;
        _logger = logger;
    }
    
    /// <summary>
    /// Retourne toutes les commandes contenues dans la base de données.
    /// </summary>
    /// <returns>La liste des commandes dans la base de données dans le corps de la requête.</returns>
    /// <response code="200">La liste des commandes est retournée dans le corps de la réponse.</response>
    /// <response code="500">Une erreur au niveau du serveur est survenue.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Commande>>> ObtenirTout()
    {
         var commandes = await _commandeService.ObtenirTout();
         _logger.LogInformation(CustomLogEvents.Lecture, "Obtention de {Compte} commande(s)", commandes.Count());
         
         return Ok(commandes);
    }
    
    /// <summary>
    /// Retourne les commandes pour un id usager contenues dans la base de données.
    /// </summary>
    /// <returns>La liste des commandes dans la base de données dans le corps de la requête.</returns>
    /// <response code="200">La liste des commandes est retournée dans le corps de la réponse.</response>
    /// <response code="500">Une erreur au niveau du serveur est survenue.</response>
    [Route("usager/{id:int}")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Commande>>> ObtenirToutPourUsager(int id)
    {
        var commandes = await _commandeService.ObtenirCommandesParUsager(id);
        _logger.LogInformation(CustomLogEvents.Lecture, "Obtention de {Compte} commande(s) pour usager id {Id}",
            commandes.Count(), id);
         
        return Ok(commandes);
    }
    
    
    /// <summary>
    /// Retourne une commande selon l'identifiant fourni dans la requête.
    /// </summary>
    /// <param name="id">L'identifiant de la commande.</param>
    /// <returns>Une réponse HTTP Ok avec une commande dans le corps de la requête ou bien une réponse HTTP NotFound.</returns>
    /// <response code="200">Une commande dans le corps de la requête est fournie.</response>
    /// <response code="404">Aucune commande avec l'identifiant fourni n'a été trouvée.</response>
    /// <response code="500">Une erreur au niveau du serveur est survenue.</response>
    [Route("{id:int}")]
    [HttpGet]
    public async Task<ActionResult<Commande>> ObtenirUne(int id)
    {
        var commande = await _commandeService.ObtenirParId(id);
        _logger.LogInformation(CustomLogEvents.Lecture ,"Obtention de la commande avec l'identifiant {Id}", id);

        if (commande is null)
        {
            _logger.LogInformation(CustomLogEvents.Lecture, "La commande avec l'identifiant {Id} n'a pas été trouvée", id);
            return NotFound();
        }
        
        _logger.LogInformation(CustomLogEvents.Lecture, "La commande avec l'identifiant {Id} va être envoyée", id);
        return Ok(commande);
    }
    
    /// <summary>
    /// Enregistre une commande passée dans le corps de la requête dans la base de données.
    /// </summary>
    /// <param name="commande">Commande à enregistrer dans la base de données.</param>
    /// <returns>Une réponse HTTP Created avec le chemin pour y accéder par l'API dans le corps de la requête.</returns>
    /// <response code="201">La création de la commande a réussi.</response>
    /// <response code="400">Une commande avec l'identifiant du véhicule fourni existe déjà.</response>
    /// <response code="400">La date d'une commande ne peut être antérieure à aujourd'hui.</response>
    /// <response code="500">Une erreur au niveau du serveur est survenue.</response>
    [Route("enregistrer")]
    [HttpPost]
    public async Task<IActionResult> Enregistrer([FromBody] Commande commande)
    {
        bool existe = await _commandeService.CommandeAvecVehiculeExiste(commande.VehiculeId);
        _logger.LogInformation(CustomLogEvents.Creation,
            "Vérification de l'existance d'une commande avec l'identifiant de véhicule {IdVehicule}", commande.VehiculeId);

        if (existe)
        {
            _logger.LogError(CustomLogEvents.Creation, 
                "Une commande est déjà existante pour le véhicule (id: {Id})", commande.VehiculeId);
            return BadRequest($"Une commande pour le véhicule (id: {commande.VehiculeId}) existe déjà dans la base de données.");
        }

        if (commande.Date.Date < DateTime.Now.Date)
        {
            _logger.LogError(CustomLogEvents.Creation, 
                "La date ({Date:yy-MM-dd}) n'est pas une date valide", commande.Date);
            return BadRequest("Veuillez enregistrer une commande avec la date actuelle d'aujourd'hui.");
        }

        await _commandeService.Enregistrer(commande);
        _logger.LogInformation(CustomLogEvents.Creation,
            "Commande enregistrée avec l'identifiant {Id} dans la base de données", commande.Id);
        
        return CreatedAtAction(nameof(ObtenirUne), new {id = commande.Id}, commande);
    }
    
    /// <summary>
    /// Modifie une commande selon l'identifiant et la commande passée en paramètre.
    /// </summary>
    /// <param name="id">Identifiant de la commande à modifier.</param>
    /// <param name="commande">Commande à modifier.</param>
    /// <returns>Une réponse HTTP NoContent quand la requête est réussie.</returns>
    /// <response code="204">La requête de modification de la commande a été effectuée.</response>
    /// <response code="400">L'identifiant passé en paramètre et l'identifiant de la commande passée en paramètre ne sont pas les mêmes.</response>
    /// <response code="400">Une commande avec l'identifiant du véhicule fourni existe déjà.</response>
    /// <response code="400">Lors d'une modification de commande, on ne peut pas changer la date.</response>
    /// <response code="404">La commande à modifier n'existe pas dans la base de données.</response>
    /// <response code="500">Une erreur au niveau du serveur est survenue.</response>
    [Route("modifier/{id:int}")]
    [HttpPut]
    public async Task<IActionResult> Modifier(int id, [FromBody] Commande commande)
    {
        if (id != commande.Id)
        {
            _logger.LogInformation(CustomLogEvents.Modification,
                "L'identifiant fourni ({Id}) ne correspond pas à la commande de la requête (id: {IdCommande})", 
                id, commande.Id);
            return BadRequest($"L'identifiant fourni ({id}) ne correspond pas à la commande passée (id: {commande.Id}) dans le corps de la requête.");
        }

        var ancienneCommande = await _commandeService.ObtenirParId(id);
        _logger.LogInformation(CustomLogEvents.Modification, 
            "Obtention de la commande avec l'identifiant {Id} pour vérification avant modification", id);

        if (ancienneCommande is null)
        {
            _logger.LogInformation(CustomLogEvents.Modification, 
                "La commande avec l'identifiant {Id} n'existe pas dans la base de données", id);
            return NotFound();
        }

        if (ancienneCommande.VehiculeId != commande.VehiculeId)
        {
            bool existe = await _commandeService.CommandeAvecVehiculeExiste(commande.VehiculeId);
            _logger.LogInformation(CustomLogEvents.Modification,
                "Vérification de l'existance d'une commande avec l'identifiant de véhicule {IdVehicule}", commande.VehiculeId);
            
            if (existe)
            {
                _logger.LogInformation(CustomLogEvents.Modification, 
                    "Une commande est déjà existante pour le véhicule (id: {Id})", commande.VehiculeId);
                return BadRequest($"Une commande pour le véhicule (id: {commande.VehiculeId}) existe déjà dans la base de données.");
            }
        }

        if (commande.Date != ancienneCommande.Date)
        {
            _logger.LogInformation(CustomLogEvents.Modification, 
                "La date de la commande modifiée ({Date:yy-MM-dd} n'est pas pareille à celle de la commande originale {DateOrig:yy-MM-dd}",
                commande.Date, ancienneCommande.Date);
            return BadRequest("La date d'une commande ne peut pas être modifiée.");
        }

        await _commandeService.Modifier(commande);
        _logger.LogInformation(CustomLogEvents.Modification, "Modification de la commande avec l'identifiant {Id}", id);

        return NoContent();
    }

    /// <summary>
    /// Supprime une commande selon l'identifiant passé en paramètre de la base de données.
    /// </summary>
    /// <param name="id">Identifiant de la commande à supprimer.</param>
    /// <returns>Une réponse HTTP NoContent si la suppression a réussi, une réponse NotFound si la commande n'est pas trouvée.</returns>
    /// <response code="204">La requête de suppression a été effectuée.</response>
    /// <response code="404">La commande à supprimer n'existe pas dans la base de données.</response>
    /// <response code="500">Une erreur au niveau du serveur est survenue.</response>
    [Route("supprimer/{id:int}")]
    [HttpDelete]
    public async Task<IActionResult> Supprimer(int id)
    {
        var commandeExistante = await _commandeService.ObtenirParId(id);
        _logger.LogInformation(CustomLogEvents.Lecture,
            "Obtention de la commande avec l'identifiant {Id} pour vérification avant suppression", id);

        if (commandeExistante is null)
        {
            _logger.LogInformation(CustomLogEvents.Suppression, 
                "La commande avec l'identifiant {Id} n'existe pas dans la base de données", id);
            return NotFound();
        }

        await _commandeService.Supprimer(commandeExistante);
        _logger.LogInformation(CustomLogEvents.Suppression, "Suppression de la commande avec l'identifiant {Id}", id);
        
        return NoContent();
    }
}