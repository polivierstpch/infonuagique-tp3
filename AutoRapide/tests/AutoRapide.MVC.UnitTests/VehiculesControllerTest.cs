using AutoFixture;
using AutoRapide.MVC.Controllers;
using AutoRapide.MVC.Interfaces;
using AutoRapide.MVC.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AutoRapide.MVC.UnitTests;

public class VehiculesControllerTest
{
    private static Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
        {"UrlFichiersAPI", "string"}
    };
    private static readonly Mock<IVehiculesService> _vehiculesService = new Mock<IVehiculesService>();
    private static readonly Mock<IFichiersService> _fichiersService = new Mock<IFichiersService>();
    private static readonly Mock<IFavorisService> _favorisService = new Mock<IFavorisService>();
    private static readonly IConfiguration _config = new ConfigurationBuilder()
    .AddInMemoryCollection(inMemorySettings)
    .Build();
    private readonly Fixture _fixture = new Fixture();
    private readonly VehiculesController _controller = new VehiculesController(_vehiculesService.Object, _fichiersService.Object, _favorisService.Object, _config);

    [Fact]
    public async void Create_Model_Invalide_Retourne_ViewCreateVehicule()
    {
        //Avec
        var vehicule = new Vehicule() { };
        var fichiers = new IFormFile[2];
        _controller.ModelState.AddModelError("Vehicule", "Champs manquants");

        //Quand
        var result = await _controller.Create(vehicule, fichiers);
        var view = result as ViewResult;

        //Alors
        view.Model.Should().Be(vehicule);
        view.ViewData.ModelState.ErrorCount.Should().BeGreaterThan(0);

    }
    [Fact]
    public async void Create_SansFichier_Invalide_Retourne_ViewCreateVehicule()
    {
        //Avec
        var vehicule = new Vehicule() { };
        IFormFile[] fichiers = null;

        //Quand
        var result = await _controller.Create(vehicule, fichiers);
        var view = result as ViewResult;

        //Alors
        view.Model.Should().Be(vehicule);
        (_controller.ViewBag.Erreur as string).Should().Be("Il faut fournir deux images de format jpeg/jpg ou png.");

    }
    [Fact]
    public async void Create_EchecApiFichiers_Lance_HttpRequestException()
    {
        //Avec
        var vehicule = _fixture.Create<Vehicule>();
        var fichiers = new IFormFile[2];
        _fichiersService.Setup(_ => _.EnvoyerFichiers(vehicule.NIV, fichiers)).ReturnsAsync(new List<string>());

        //Quand
        Func<Task> action = async () => await _controller.Create(vehicule, fichiers);

        //Alors
        action.Should().ThrowAsync<HttpRequestException>().WithMessage("Une erreur est survenue lors de l'envoi des fichiers.");

    }

    [Fact]
    public async void Create_ModelEtFichiersOK_SuccesAjout_Retourne_ViewIndexVehiculesEtOkStatusCode()
    {
        //Avec
        var vehicule = _fixture.Create<Vehicule>();
        var fichiers = new IFormFile[2];
        _fichiersService.Setup(_ => _.EnvoyerFichiers(It.IsAny<string>(), It.IsAny<IFormFile[]>())).ReturnsAsync(() => new List<String>() { "string1", "string2" }) ;
        _vehiculesService.Setup(_ => _.AjouterAsync(vehicule)).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.Created));


        //Quand
        var result = await _controller.Create(vehicule, fichiers);

        //Alors
        (result as RedirectToActionResult).Should().NotBe(null);
        (result as RedirectToActionResult).ActionName.Should().Be("Index");

    }

    [Fact]
    public async void Create_ModelEtFichiersOK_EchecAjout_Lance_HttpRequestException()
    {
        //Avec
        var vehicule = _fixture.Create<Vehicule>();
        var fichiers = new IFormFile[2];
        _fichiersService.Setup(_ => _.EnvoyerFichiers(It.IsAny<string>(), It.IsAny<IFormFile[]>())).ReturnsAsync(() => new List<String>() { "string1", "string2"});
        
        _vehiculesService.Setup(_ => _.AjouterAsync(vehicule)).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));

        //Quand
        Func<Task> action = async () => await _controller.Create(vehicule, fichiers);

        //Alors
        action.Should().ThrowAsync<HttpRequestException>();

    }
}
