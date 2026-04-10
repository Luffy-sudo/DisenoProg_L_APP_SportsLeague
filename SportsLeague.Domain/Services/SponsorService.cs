using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;

        public SponsorService(
            ISponsorRepository sponsorRepository, 
            ITournamentSponsorRepository tournamentSponsorRepository)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _sponsorRepository.GetAllAsync();
        }

        // 1. Fix: Implementación de GetByIdAsync
        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            return await _sponsorRepository.GetByIdAsync(id);
        }

        // 2. Fix: Implementación de UpdateAsync
        public async Task UpdateAsync(Sponsor sponsor)
        {
            await _sponsorRepository.UpdateAsync(sponsor);
        }

        // 3. Fix: Implementación de DeleteAsync
        public async Task DeleteAsync(int id)
        {
            await _sponsorRepository.DeleteAsync(id);
        }

        public async Task CreateSponsorAsync(Sponsor sponsor)
        {
            // Validación de nombre duplicado
            if (await _sponsorRepository.ExistsByNameAsync(sponsor.Name))
            {
                throw new InvalidOperationException("A sponsor with the same name already exists.");
            }

            await _sponsorRepository.CreateAsync(sponsor);
        }

        public async Task LinkToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount)
        {
            // Validación de monto
            if (contractAmount <= 0)
                throw new ArgumentException("Contract amount must be greater than zero.");

            // Validación de existencia de relación previa
            if (await _tournamentSponsorRepository.IsAlreadyLinked(sponsorId, tournamentId))
                throw new InvalidOperationException("This sponsor is already linked to the tournament.");

            await _tournamentSponsorRepository.AddLinkAsync(sponsorId, tournamentId, contractAmount);
        }
    }
}