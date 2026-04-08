using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    // Debe ser public para que el proyecto DataAccess pueda verla
    public interface ISponsorRepository : IGenericRepository<Sponsor>
    {
        // Método específico requerido para validar nombres duplicados
        Task<bool> ExistsByNameAsync(string name);
        Task<Sponsor?> GetByNameAsync(string name);
    }
}