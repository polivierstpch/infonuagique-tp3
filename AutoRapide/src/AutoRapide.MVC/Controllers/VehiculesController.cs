using AutoRapide.MVC.Interfaces;
using AutoRapide.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoRapide.MVC.Controllers;

public class VehiculesController : Controller
{
    private readonly IVehiculesService _vehiculesService;
    private readonly IFichiersService _fichiersService;
    private readonly IFavorisService _favorisService;
    private readonly IConfiguration _config;

    public VehiculesController(
        IVehiculesService vehiculesService,
        IFichiersService fichiersService, 
        IFavorisService favorisService,
        IConfiguration config
    )
    {
        _vehiculesService = vehiculesService;
        _fichiersService = fichiersService;
        _favorisService = favorisService;
        _config = config;
    }

    // GET
    public async Task<IActionResult> Index(bool? prixDescendant, string filtre = "")
    {
        var vehicules = await _vehiculesService.ObtenirToutAsync();

        if (!string.IsNullOrWhiteSpace(filtre))
        {
            filtre = filtre.ToLower();
            
            vehicules = vehicules.Where(v =>
            {
                var modeleConstructeur = $"{v.Constructeur} {v.Modele}".ToLower();
                return v.Constructeur.ToLower().Contains(filtre) 
                       || v.Modele.ToLower().Contains(filtre)
                       || modeleConstructeur.Contains(filtre);
            });
            
            ViewBag.FiltreActuel = filtre;
        }
        
        
        ViewBag.PrixDescendant = prixDescendant;
        
        if(prixDescendant is null)
            return View(vehicules);

        if (prixDescendant.Value)
            vehicules = vehicules.OrderByDescending(v => v.Prix);
        else
            vehicules = vehicules.OrderBy(v => v.Prix);
        
        return View(vehicules);
    }

    private bool ConstructeurOuModeleContient(Vehicule vehicule, string[] valeurs)
    {
        bool constructeurContient = false;
        bool modeleContient = false;
        
        foreach (var valeur in valeurs)
        {
            constructeurContient = vehicule.Constructeur.ToLower().Contains(valeur);
            modeleContient = vehicule.Constructeur.ToLower().Contains(valeur);
        }

        return constructeurContient || modeleContient;
    }

    public async Task<IActionResult> Details(int id)
    {
        var vehicule = await _vehiculesService.ObtenirParIdAsync(id);

        if (vehicule is null)
            return NotFound();
        
        var favoris = await _favorisService.ObtenirLesFavoris();
        ViewBag.IsFavori = favoris.Contains(id);
        ViewBag.IdVehicule = id;
        
        return View(vehicule);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Constructeur", "Modele", "AnneeFabrication", "Type", "NombreSiege", "Couleur", "Description", "Prix")]
        Vehicule vehicule,
        IFormFile[] fichiers
    )
    {
        if (!ModelState.IsValid)
            return View(vehicule);

        if (fichiers is null || fichiers.Length != 2)
        {
            ViewBag.Erreur = "Il faut fournir deux images de format jpeg/jpg ou png.";
            return View(vehicule);
        }

        vehicule.NIV = ConstruireNIV(vehicule);

        var urlsFichiers = (await _fichiersService.EnvoyerFichiers(vehicule.NIV, fichiers));
        var urlsFichiersList = urlsFichiers.ToList();
        
        if (urlsFichiersList.Count != 2)
        {
            throw new HttpRequestException("Une erreur est survenue lors de l'envoi des fichiers.");
        }

        var fichierApiUrl = _config.GetValue<string>("UrlStorageAccount");
        vehicule.Image1Url = fichierApiUrl + urlsFichiersList[0];
        vehicule.Image2Url = fichierApiUrl + urlsFichiersList[1];

        vehicule.EstDisponible = true;

        var reponse = await _vehiculesService.AjouterAsync(vehicule);

        return RedirectToAction(nameof(Index));
    }
    
    private string ConstruireNIV(Vehicule vehicule)
    {
        string codeConstructeur = vehicule.Constructeur.Trim()[..3];
        string codeSiege = vehicule.NombreSiege.ToString("D2");
        string typeVehicule = ((int)vehicule.Type).ToString("D2");
        
        string anneeRaccourcie = vehicule.AnneeFabrication.ToString()[2..];
        string codeAnneeModel = $"{vehicule.Modele.Trim().Replace("-", "")[..2]}{anneeRaccourcie}";
        string numero = vehicule.Id.ToString("D6");
        
        return $"{codeConstructeur}{codeSiege}{typeVehicule}{codeAnneeModel}{numero}".ToUpper();
    }
    
}