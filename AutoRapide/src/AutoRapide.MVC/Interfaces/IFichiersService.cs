namespace AutoRapide.MVC.Interfaces
{
    public interface IFichiersService
    {
        public Task<IEnumerable<string>> EnvoyerFichiers(string niv, IEnumerable<IFormFile> fichiers);
    }
}
