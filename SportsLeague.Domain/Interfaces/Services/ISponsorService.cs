using SportsLeague.Domain.Entities;

public interface ISponsorService
{
    // El tipo de retorno debe ser Task para que sea asíncrono
    Task<IEnumerable<Sponsor>> GetAllAsync();
    
    Task CreateSponsorAsync(Sponsor sponsor);
    Task LinkToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount);

    Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tournamentId);

    Task<Sponsor?> GetByIdAsync(int id);

    Task UpdateAsync(Sponsor sponsor);

    Task DeleteAsync(int id);
}