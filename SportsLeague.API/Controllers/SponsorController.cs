using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;

namespace SportsLeague.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _sponsorService;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService sponsorService, IMapper mapper)
        {
            _sponsorService = sponsorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
        {
            var sponsors = await _sponsorService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
        {
            var sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null) return NotFound();
            
            return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
        }

        [HttpPost]
        public async Task<ActionResult<SponsorResponseDTO>> Create(SponsorRequestDTO request)
        {
            var sponsor = _mapper.Map<Sponsor>(request);
            await _sponsorService.CreateSponsorAsync(sponsor);
            
            var response = _mapper.Map<SponsorResponseDTO>(sponsor);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SponsorRequestDTO request)
        {
            var existing = await _sponsorService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(request, existing);
            await _sponsorService.UpdateAsync(existing);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _sponsorService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _sponsorService.DeleteAsync(id);
            return NoContent();
        }

        // Endpoint extra para la vinculación
        [HttpPost("link-tournament")]
        public async Task<IActionResult> LinkToTournament(LinkSponsorRequestDTO request)
        {
            await _sponsorService.LinkToTournamentAsync(
                request.SponsorId, 
                request.TournamentId, 
                request.ContractAmount);
                
            return Ok(new { message = "Sponsor linked successfully to tournament." });
        }

        [HttpGet("tournament/{tournamentId}")]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetSponsorsByTournament(int tournamentId)
        {
            var links = await _sponsorService.GetSponsorsByTournamentAsync(tournamentId);
            var response = _mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(links);
            return Ok(response);
        }

    }
}