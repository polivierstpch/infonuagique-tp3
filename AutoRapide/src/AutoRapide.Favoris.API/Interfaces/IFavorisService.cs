namespace AutoRapide.Favoris.API.Interfaces
{
    public interface IFavorisService
    {
        IEnumerable<int> ObtenirLesFavoris(string ip);
        void AjouterFavori(int idVehicule, string ip);
        void EffacerFavori(int idVehicule, string ip);
    }
}
