## Enunciado del problema

La Liga Deportiva necesita gestionar los patrocinadores que financian los torneos. Cada torneo puede tener múltiples patrocinadores y cada patrocinador puede participar en múltiples torneos. Tu tarea es implementar TODO EL FLUJO COMPLETO DE LA ENTIDAD SPONSOR DENTRO DEL PROYECTO SportsLeague existente, siguiendo los mismos patrones y estructura de las fases anteriores.<br/><br/>

### Requerimientos Técnicos <br/><br/>

#### [Entidad Sponsor (Patrocinador)](SportsLeague.Domain\Entities\Sponsor.cs)

Crear la entidad _Sponsor_ con sus respectivas propiedades.<br/><br>

#### [Enum SponsorCategory](SportsLeague.Domain\Enums\SponsorCategory.cs)

Crear el _enum_ con las siguientes categorías:

```C#

public enum SponsorCategory
{
    Main = 0,     // Patrocinador principal
    Gold = 1,     // Oro
    Silver = 2,  // Plata
    Bronze = 3   // Bronce
}

```

#### Relación N:M — [TournamentSponsor](SportsLeague.Domain\Entities\TournamentSponsor.cs)

Crear la tabla intermedia _TournamentSponsor_ para la relación muchos a muchos entre [Tournament](SportsLeague.Domain\Entities\Tournament.cs) y [Sponsor](SportsLeague.Domain\Entities\Sponsor.cs) con sus respectivas propiedades.<br/><br>

**La relación N:M de TournamentSponsor sigue el mismo patrón de [TournamentTeam](SportsLeague.Domain\Entities\TournamentTeam.cs). Recuerda configurar las FK y Navigation Properties.**<br/><br/>

#### CheckList <br/><br/>

**1.** Entity [Sponsor](SportsLeague.Domain\Entities\Sponsor.cs) + [Enum SponsorCategory](SportsLeague.Domain\Enums\SponsorCategory.cs)<br/><br>

**2.** Configuración en el [DbContext (OnModelCreating)](SportsLeague.DataAccess\Context\LeagueDbContext.cs): agregar el _DbSet<#Sponsor>_ y configurar un índice único en la propiedad _Name_ para que no se puedan crear dos sponsors con el mismo nombre.

```C#

 public DbSet<Sponsor> Sponsors => Set<Sponsor>();

            // ── Sponsor Configuration ──
            modelBuilder.Entity<Sponsor>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(s => s.ContactEmail)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(s => s.Phone)
                      .HasMaxLength(20);
                entity.Property(s => s.WebsiteUrl)
                      .HasMaxLength(500);
                entity.Property(s => s.Category)
                      .IsRequired();
                entity.Property(s => s.CreatedAt)
                      .IsRequired();
                entity.Property(s => s.UpdatedAt)
                      .IsRequired(false);

                // Índice único en el nombre del patrocinador  <------------
                entity.HasIndex(s => s.Name)
                      .IsUnique();
            });

```

**3.** [ISponsorRepository](SportsLeague.Domain\Interfaces\Repositories\ISponsorRepository.cs) + [SponsorRepository](SportsLeague.DataAccess\Repositories\SponsorRepository.cs)

El Repository hereda de GenericRepository<#Sponsor> para obtener el CRUD base, y agrega los métodos específicos que necesite el Service para las validaciones. Las dependencias se registran en la clase [Program.cs](SportsLeague.API\Program.cs).

_La herencia en C# permite que una clase hija adquiera automáticamente los miembros (campos, propiedades, métodos) de una clase (padre), facilitando la reutilización de código y creando relaciones jerárquicas. Se implementa usando dos puntos (class Hija: Padre). La clase hija puede añadir, modificar o reutilizar los miembros recibidos._

```C#

using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    // Debe ser public para que el proyecto DataAccess pueda verla
    public interface ISponsorRepository : IGenericRepository<Sponsor> //ISponsorRepository hereda de IGenericRepository<Sponsor>
    {
        // Método específico requerido para validar nombres duplicados
        Task<bool> ExistsByNameAsync(string name);
    }
}


```

<br/><br/>

**Registro de dependencias en la clase [Program.cs](SportsLeague.API\Program.cs)**

```C#

// ── Repositories ──
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IRefereeRepository, RefereeRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentTeamRepository, TournamentTeamRepository>();
builder.Services.AddScoped<ISponsorRepository, SponsorRepository>();

// ── Services ──
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IRefereeService, RefereeService>();
builder.Services.AddScoped<ITournamentService, TournamentService>();

```

<br/>

El método _AddScoped_ se utiliza para registrar servicios con un tiempo de vida de duración de solicitud (scoped).

Aquí se están aplicando dos formas distintas de usar este método para gestionar la inyección de dependencias (DI):

**3.1. Registro de Genéricos Abiertos (Open Generics)**

```C#
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
```

Aquí se utiliza una sobrecarga que acepta objetos Type. Le dice al contenedor de DI que, cada vez que alguien solicite un _<IgenericRepository<"T">>_ (donde T es cualquier entidad, como User o Product), debe crear una instancia de GenericRepository<"T">. Esto evita tener que registrar manualmente cada repositorio base para cada entidad de la base de datos.<br/><br>

**3.2. Registro de Tipos Específicos (Typed Services)**

```C#
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
```

Esta es la forma más común y utiliza tipos genéricos de C#. Mapea una interfaz (el contrato) a una implementación concreta. Cuando el sistema pide _ITeamRepository_, el contenedor sabe que debe entregar una instancia _TeamRepository_. Esto permite desacoplar el código y facilitar las pruebas unitarias (_Mocking_).

Al usar el método _AddScopped_ se crea una única instancia de _TeamService_ dentro de una misma petición web. Si varias clases dentro de esa misma petición necesitan el serviciio, compartirán esa misma instancia.<br/><br>

**4.** [ISponsorService](SportsLeague.Domain\Interfaces\Services\ISponsorService.cs) + [SponsorService](SportsLeague.Domain\Services\SponsorService.cs): El _Service_ recibe el [ISponsorRepository](SportsLeague.Domain\Interfaces\Repositories\ISponsorRepository.cs) por inyección de dependencias y contiene toda la lógica de negocio: crear, obtener, actualizar, eliminar, con las validaciones de negocio requeridas. Registrar en [Program.cs](SportsLeague.API\Program.cs).<br/><br>

**5.** [SponsorRequestDTO](SportsLeague.API\DTOs\Request\SponsorRequestDTO.cs) (lo que envía el cliente al crear o actualizar) y [SponsorResponseDTO](SportsLeague.API\DTOs\Response\SponsorResponseDTO.cs) (lo que retorna la API).<br/><br>

**6. Perfil de AutoMapper**: agregar los mapeos Sponsor → SponsorResponseDTO y SponsorRequestDTO → Sponsor en el [MappingProfile](SportsLeague.API\Mappings\MappingProfile.cs) existente.<br/><br>

**7.** [SponsorController](SportsLeague.API\Controllers\SponsorController.cs) con los 5 endpoints CRUD: GET all, GET by id, POST, PUT, DELETE.

\*\*Para la relación N:M
