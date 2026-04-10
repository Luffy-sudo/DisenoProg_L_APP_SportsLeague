using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Domain.DTOs.Request
{
    public class SponsorRequestDTO
    {

        public string Name { get; set; } = string.Empty;

  
        public string ContactEmail { get; set; } = string.Empty;

  
        public string? Phone { get; set; }


        public string? WebsiteUrl { get; set; }

     
        public string Category { get; set; } = string.Empty;
    }
}