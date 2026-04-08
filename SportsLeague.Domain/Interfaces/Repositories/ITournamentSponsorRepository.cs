namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository
    {
        Task<bool> IsAlreadyLinked(int sponsorId, int tournamentId);
        Task AddLinkAsync(int sponsorId, int tournamentId, decimal amount);
        // Agrega aquí los métodos de IGenericRepository si los heredas
    }
}