using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository: IGenericRepository<TournamentSponsor>
    {
        Task<bool> IsAlreadyLinked(int sponsorId, int tournamentId);
        Task AddLinkAsync(int sponsorId, int tournamentId, decimal amount);
     
        Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId);

        Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);

    }
}