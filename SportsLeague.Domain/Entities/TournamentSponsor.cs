namespace SportsLeague.Domain.Entities
{
    public class TournamentSponsor : AuditBase
    {
        public int TournamentId { get; set; } //FK
        public int SponsorId { get; set; } //FK
        public decimal ContractAmount { get; set; }
        public DateTime JoinedAt { get; set; }

        // Navigation Properties for the Foreign Keys
        public Tournament Tournament { get; set; } = null!;
        public Sponsor Sponsor { get; set; } = null!;
    }
}