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

/*

TournamentSponsor funciona como una tabla de unión que contiene
las llaves foráneas. Al definir estas propiedades con = null!, 
permite que Entity Framework realice un Join
automático, lo que hace posible navegar desde el vínculo hacia la 
información completa del patrocinador o del torneo.

*/