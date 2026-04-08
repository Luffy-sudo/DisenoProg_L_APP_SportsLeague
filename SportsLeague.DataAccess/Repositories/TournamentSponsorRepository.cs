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

        /// Verifica si ya existe una relación activa entre un patrocinador y un torneo.
     
   
        public async Task<bool> IsAlreadyLinked(int sponsorId, int tournamentId)
        {
            return await _context.TournamentSponsors
                .AnyAsync(ts => ts.SponsorId == sponsorId && ts.TournamentId == tournamentId);
        }

        /// Crea la vinculación en la tabla intermedia con el monto del contrato.
        public async Task AddLinkAsync(int sponsorId, int tournamentId, decimal amount)
        {
            var tournamentSponsor = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = amount
            };

            // Usamos el contexto directamente o el método CreateAsync heredado
            await _context.TournamentSponsors.AddAsync(tournamentSponsor);
            await _context.SaveChangesAsync();
        }
    }
}