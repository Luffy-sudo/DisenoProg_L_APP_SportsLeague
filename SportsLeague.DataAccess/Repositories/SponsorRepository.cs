using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context; 
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        private readonly LeagueDbContext _context; // Cambiado de AppDbContext a LeagueDbContext

        public SponsorRepository(LeagueDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Sponsor?> GetByNameAsync(string name)
        {
            return await _context.Sponsors
                .FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Sponsors
                .AnyAsync(s => s.Name == name);
        }
    }
}