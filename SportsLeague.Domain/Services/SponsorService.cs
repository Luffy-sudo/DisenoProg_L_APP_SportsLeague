using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Business.Services // Asegúrate de tener tu namespace
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;

        public SponsorService(
            ISponsorRepository sponsorRepository, 
            ITournamentRepository tournamentRepository,
            ITournamentSponsorRepository tournamentSponsorRepository)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentRepository = tournamentRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
        }

        // Esta es la implementación válida que cumple con la interfaz
        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _sponsorRepository.GetAllAsync();
        }

        public async Task CreateSponsorAsync(Sponsor sponsor)
        {
            var existing = await _sponsorRepository.GetByNameAsync(sponsor.Name);
            if (existing != null) 
                throw new InvalidOperationException("El nombre del patrocinador ya existe.");

            if (!IsValidEmail(sponsor.ContactEmail))
                throw new InvalidOperationException("El formato del email no es válido.");

            await _sponsorRepository.CreateAsync(sponsor);
        }

        public async Task LinkToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount)
        {
            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
            if (sponsor == null) throw new KeyNotFoundException("Sponsor no encontrado.");

            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null) throw new KeyNotFoundException("Tournament no encontrado.");

            var alreadyLinked = await _tournamentSponsorRepository.IsAlreadyLinked(sponsorId, tournamentId);
            if (alreadyLinked)
                throw new InvalidOperationException("El patrocinador ya está vinculado a este torneo.");

            if (contractAmount <= 0)
                throw new InvalidOperationException("El monto del contrato debe ser mayor a 0.");

            await _tournamentSponsorRepository.AddLinkAsync(sponsorId, tournamentId, contractAmount);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
        }
    }
}