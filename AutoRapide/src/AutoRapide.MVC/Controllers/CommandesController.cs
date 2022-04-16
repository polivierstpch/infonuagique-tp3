using AutoRapide.MVC.Interfaces;
using AutoRapide.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoRapide.MVC.Controllers;

[Route("commander")]
public class CommandesController : Controller
{
    private readonly ICommandesService _commandesService;
    private readonly IVehiculesService _vehiculesService;
    private readonly IUsagerService _usagerService;
    private readonly ILogger<CommandesController> _logger;

    public CommandesController(
        ICommandesService commandes,
        IVehiculesService vehiculesService,
        ILogger<CommandesController> logger,
        IUsagerService usagerService
    )
    {
        _commandesService = commandes;
        _vehiculesService = vehiculesService;
        _logger = logger;
        _usagerService = usagerService;
    }
    
    public async Task<IActionResult> Create(int id)
    {
        var vehicule = await _vehiculesService.ObtenirParIdAsync(id);

        if (vehicule is null)
        {
            return NotFound();
        }

        return View(vehicule);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Vehicule vehicule, string codeUsager)
    {
        if (!vehicule.EstDisponible)
        {
            ViewBag.Erreur = "Le véhicule demandé n'est pas disponible.";
            return View(vehicule);
        }
        
        if (string.IsNullOrEmpty(codeUsager))
        {
            ViewBag.Erreur = "Le code usager ne peut être vide.";
            return View(vehicule);
        }
        
        var usager = await _usagerService.ObtenirUsagerParCodeUsager(codeUsager);
        if (usager is null)
        {
            ViewBag.Erreur = "Le code usager est invalide.";
            return View(vehicule);
        }

        await _commandesService.AjouterAsync(new Commande { UsagerId = usager.Id, VehiculeId = vehicule.Id });
        vehicule.EstDisponible = false;
        var result = await _vehiculesService.ModifierAsync(vehicule);

        if (result.IsSuccessStatusCode)
            return RedirectToAction("Index", "Vehicules", new { id = vehicule.Id });

        ViewBag.Erreur = "Une erreur est survenue lors de la prise de la commande.";
    
        vehicule.EstDisponible = true;
        return View(vehicule);
    }
    

}