using SportsLeague.Domain.Entities;

public interface ISponsorService
{
    // El tipo de retorno debe ser Task para que sea asíncrono
    Task<IEnumerable<Sponsor>> GetAllAsync();
    
    Task CreateSponsorAsync(Sponsor sponsor);
    Task LinkToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount);
}