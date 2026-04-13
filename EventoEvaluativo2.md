## Enunciado del problema

La Liga Deportiva necesita gestionar los patrocinadores que financian los torneos. Cada torneo puede tener múltiples patrocinadores y cada patrocinador puede participar en múltiples torneos. Tu tarea es implementar TODO EL FLUJO COMPLETO DE LA ENTIDAD SPONSOR DENTRO DEL PROYECTO SportsLeague existente, siguiendo los mismos patrones y estructura de las fases anteriores.<br/><br/>

[Entidad Sponsor (Patrocinador)](SportsLeague.Domain\Entities\Sponsor.cs)

[Enum SponsorCategory](SportsLeague.Domain\Enums\SponsorCategory.cs)

[TournamentSponsor](SportsLeague.Domain\Entities\TournamentSponsor.cs) con sus respectivas propiedades. Debe tener dos FKs (TournamentId y SponsorId) con sus respectivas Navigation Properties.

**2.** Configuración en el [DbContext (OnModelCreating)](SportsLeague.DataAccess\Context\LeagueDbContext.cs): agregar el _DbSet<#Sponsor>_ y configurar un índice único en la propiedad _Name_ para que no se puedan crear dos sponsors con el mismo nombre.

**3.** [ISponsorRepository](SportsLeague.Domain\Interfaces\Repositories\ISponsorRepository.cs) + [SponsorRepository](SportsLeague.DataAccess\Repositories\SponsorRepository.cs)

El Repository hereda de GenericRepository<#Sponsor> para obtener el CRUD base, y agrega los métodos específicos que necesite el Service para las validaciones. Las dependencias se registran en la clase [Program.cs](SportsLeague.API\Program.cs).

[ITournamentSponsorRepository](SportsLeague.Domain\Interfaces\Repositories\ITournamentSponsorRepository.cs) + [TournamentSponsorRepository](SportsLeague.DataAccess\Repositories\TournamentSponsorRepository.cs): Este Repository necesita métodos específicos

**Registro de dependencias en la clase [Program.cs](SportsLeague.API\Program.cs)**

**4.** [ISponsorService](SportsLeague.Domain\Interfaces\Services\ISponsorService.cs) + [SponsorService](SportsLeague.Domain\Services\SponsorService.cs): El _Service_ recibe el [ISponsorRepository](SportsLeague.Domain\Interfaces\Repositories\ISponsorRepository.cs) por inyección de dependencias y contiene toda la lógica de negocio: crear, obtener, actualizar, eliminar, con las validaciones de negocio requeridas. Registrar en [Program.cs](SportsLeague.API\Program.cs).

**5.** [SponsorRequestDTO](SportsLeague.API\DTOs\Request\SponsorRequestDTO.cs) (lo que envía el cliente al crear o actualizar) y [SponsorResponseDTO](SportsLeague.API\DTOs\Response\SponsorResponseDTO.cs) (lo que retorna la API).<br/><br>

**6. Perfil de AutoMapper**: agregar los mapeos Sponsor → SponsorResponseDTO y SponsorRequestDTO → Sponsor en el [MappingProfile](SportsLeague.API\Mappings\MappingProfile.cs) existente.<br/><br>

**7.** [SponsorController](SportsLeague.API\Controllers\SponsorController.cs) con los 5 endpoints CRUD: GET all, GET by id, POST, PUT, DELETE.<br/><br>

**Para la relación N:M TournamentSponsor**

**1.**

**2.** Configuración en el [DbContext](SportsLeague.DataAccess\Context\LeagueDbContext.cs): agregar el DbSet <'TournamentSponsor'>, configurar las dos FKs con HasOne().WithMany().HasForeignKey(), y crear un índice único compuesto en [TournamentId, SponsorId] para evitar que un Sponsor se vincule dos veces al mismo torneo.

**3.**
