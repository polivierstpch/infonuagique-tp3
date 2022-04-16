using AutoRapide.MVC.Interfaces;
using AutoRapide.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoRapide.MVC.Controllers
{
    public class FavorisController : Controller
    {
        private readonly IFavorisService _favorisProxy;
        private readonly IVehiculesService _vehiculeProxy;

        public FavorisController(IFavorisService favorisProxy, IVehiculesService vehiculeProxy)
        {
            _favorisProxy = favorisProxy;
            _vehiculeProxy = vehiculeProxy;
        }

        // GET: FavorisController
        public async Task<ActionResult> Index()
        {
            var favoris = (await  _favorisProxy.ObtenirLesFavoris());
            var vehicules = new List<Vehicule>();
            foreach(var idVehicule in favoris)
            {
                var vehicule = await _vehiculeProxy.ObtenirParIdAsync(idVehicule);
                if(vehicule != null && !vehicules.Contains(vehicule))
                {
                    vehicules.Add(vehicule);
                }
            }
            return View(vehicules);
        }

        public async Task<IActionResult> Ajouter(int idvehicule, bool ajout)
        {
            if (ajout)
            {
                await _favorisProxy.AjouterFavori(idvehicule);
            }
            else
            {
                await _favorisProxy.EffacerFavori(idvehicule);
            }
            return RedirectToAction("Details", "Vehicules", new { id = idvehicule});
        }
    }
}
