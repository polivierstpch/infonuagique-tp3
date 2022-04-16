namespace AutoRapide.MVC.Interfaces
{
    public interface IFavorisService
    {
        Task<IEnumerable<int>> ObtenirLesFavoris();
        Task<HttpResponseMessage> AjouterFavori(int idVehicule);
        Task<HttpResponseMessage> EffacerFavori(int idVehicule);
    }
}
