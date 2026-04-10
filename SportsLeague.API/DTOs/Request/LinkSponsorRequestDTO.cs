namespace SportsLeague.Domain.DTOs.Request
{
    public class LinkSponsorRequestDTO
    {
        public int SponsorId { get; set; }

        public int TournamentId { get; set; }

        public decimal ContractAmount { get; set; }
    }
}