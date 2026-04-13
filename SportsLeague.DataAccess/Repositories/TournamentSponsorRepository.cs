using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        private readonly LeagueDbContext _context;

        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsAlreadyLinked(int sponsorId, int tournamentId)
        {
            return await _context.TournamentSponsors
                .AnyAsync(ts => ts.SponsorId == sponsorId && ts.TournamentId == tournamentId);
        }

        public async Task AddLinkAsync(int sponsorId, int tournamentId, decimal amount)
        {
            var link = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = amount,
                JoinedAt = DateTime.Now
            };
            await _context.TournamentSponsors.AddAsync(link);
            await _context.SaveChangesAsync();
        }

        // Para listar Sponsors de un Torneo (Corrigiendo el sponsorName: null)
        public async Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId)
        {
            return await _context.TournamentSponsors
                .Include(ts => ts.Sponsor) 
                .Where(ts => ts.TournamentId == tournamentId)
                .ToListAsync();
        }

        // Para listar Torneos de un Sponsor (Lo que pide la guía)
        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
        {
            return await _context.TournamentSponsors
                .Include(ts => ts.Tournament)
                .Where(ts => ts.SponsorId == sponsorId)
                .ToListAsync();
        }
    }
}