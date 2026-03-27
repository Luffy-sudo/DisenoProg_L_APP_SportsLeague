#### ¿Qué vamos a construir?

Una [API REST](APP_SportsLeague.md#api-rest) completa para la gestión de una Liga Deportiva. Esta aplicación permitirá administrar todos los aspectos de un campeonato de fútbol: desde la inscripción de equipos y jugadores, hasta el registro de partidos, goles, tarjetas y la generación automática de la tabla de posiciones. <br><br>

#### Stack Tecnológico

**Backend**

- [.NET 8](APP_SportsLeague.md#dotnet8)
- [C#](APP_SportsLeague.md#csharp)
- [Entity Framework Core 8](APP_SportsLeague.md#EFCoreCodeFirst)
- SQL Server
- [AutoMapper](APP_SportsLeague.md#AutoMapper)
- [Swagger](APP_SportsLeague.md#Swagger)
- [Swashbuckle](APP_SportsLeague.md#Swashbuckle)

**Nota Importante:** Primero construiremos toda la API completa y funcional. Una vez terminada, conectaremos el frontend en Angular. Esto nos permite probar toda la lógica de negocio de forma independiente usando Swagger o Postman.<br><br>

#### Arquitectura del proyecto

La aplicación seguirá una arquitectura de N-capas con tres proyectos independientes dentro de una misma [solución de .NET](APP_SportsLeague.md#dotnet_solution).<br/><br/>

#### _Las tres capas_<br/><br/>

**Capa de presentación**: [SportsLeague.API](SportsLeague.API)

Es el punto de entrada de la aplicación. Recibe las [peticiones HTTP](APP_SportsLeague.md#HTTP_Request) del frontend (o de Swagger/Postman), las procesa y devuelve respuestas JSON.

- [Controllers](APP_SportsLeague.md#Controllers): reciben las peticiones y delegan al service correspondiente.<br/><br/>

- [DTOs](APP_SportsLeague.md#DTO): definen la estructura de datos que se envía y se recibe del frontend. Nunca se exponen las Entities directamente.<br/><br/>

- [Mappings](APP_SportsLeague.md#Object-to-Object-Mapping): configuración de AutoMapper para convertir entre DTOs y Entities.<br/><br/>

- [Middlewares](APP_SportsLeague.md#Middleware): manejo global de errores y logging.<br/><br/>

- [Programs.cs](SportsLeague.API/Program.cs): configuración de servicios e inyección de dependencias.<br/><br/>

**Capa de Negocio**: [SportsLeague.Domain](SportsLeague.Domain/)

Es el corazón de la aplicación. Contiene toda la lógica de negocio y las reglas del dominio. Esta capa NO depende de ninguna otra: no conoce la base de datos ni el framework web.<br/>

- Entities: las clases que representan las tablas

- [Enums](APP_SportsLeague.md#Enum): tipos numerados como [PlayerPosition](SportsLeague.Domain\Enums\PlayerPosition.cs), [TournamentStatus](SportsLeague.Domain\Enums\TournamentStatus.cs)

- [Interfaces](APP_SportsLeague.md#Interfaz): contratos que definen qué operaciones existen ([ITeamRepository](SportsLeague.Domain\Interfaces\Repositories\ITeamRepository.cs), [ITeamService](SportsLeague.Domain\Interfaces\Services\ITeamService.cs)). Las implementaciones concretas están en otras capas.

- Services: clases que implementan la lógica de negocio. Validan datos, aplican reglas y coordinan las operaciones con los repositorios.

**Capa de Acceso a Datos**: [SportsLeague.DataAccess](SportsLeague.DataAccess/)

Se encarga exclusivamente de la comunicación con la base de datos SQL Server a través de [Entity Framework Core](APP_SportsLeague.md#EFCoreCodeFirst).

- [LeagueDbContext.cs](SportsLeague.DataAccess\Context\LeagueDbContext.cs): clase que configura la conexión a la base de datos y define las tablas (DbSets).

- [Repositories](SportsLeague.DataAccess/Repositories/): implementaciones concretas de las interfaces definidas en [Domain](SportsLeague.Domain). Aquí se ejecutan los queries con [LINQ](APP_SportsLeague.md#LINQ) y EF Core.

- [Migrations](SportsLeague.DataAccess/Migrations/): archivos generados automáticamente por EF Core para crear y actualizar el esquema de la base de datos.<br/><br/>

#### Flujo de una petición

Cuando el frontend (o Swagger) hace una petición HTTP a nuestra API, este es el recorrido que sigue:

1. **El frontend envía: POST /api/Team con un [JSON](APP_SportsLeague.md#JSON) ([TeamRequestDTO](SportsLeague.API\DTOs\Request\TeamRequestDTO.cs))<br/><br/>**

2. **[TeamController](SportsLeague.API\Controllers\TeamController.cs) recibe el DTO**<br/><br/>

   → Usa AutoMapper para convertir [TeamRequestDTO](SportsLeague.API\DTOs\Request\TeamRequestDTO.cs) → [Team](SportsLeague.Domain\Entities\Team.cs) (Entity)

   → Llama a [\_teamRepository.CreateAsync(team)](SportsLeague.Domain\Services\TeamService.cs)<br><br/>

3. **[TeamService](SportsLeague.Domain\Services\TeamService.cs) recibe la Entity Team**<br/><br/>

   → Valida reglas de negocio (nombre único, datos correctos)

   → Llama a [\_teamRepository.CreateAsync(team)](SportsLeague.Domain\Services\TeamService.cs)<br><br/>

4. **[TeamRepository](SportsLeague.DataAccess\Repositories\TeamRepository.cs) recibe la Entity Team**<br/><br/>

→ Usa Entity Framework Core para insertar en la tabla Teams de SQL Server

→ Retorna la entidad con el Id generado<br><br/>

5. **La respuesta viaja de vuelta:**

[Repository](SportsLeague.DataAccess\Repositories) → [Service](SportsLeague.Domain\Services) → [Controller](SportsLeague.API\Controllers) → AutoMapper (Team → [TeamResponseDTO](SportsLeague.API\DTOs\Response\TeamResponseDTO.cs)) → JSON al frontend <br/><br/><br/>

#### Referencias entre proyectos <br><br/>

[SportsLeague.API](SportsLeague.API) → referencia a → [SportsLeague.Domain](SportsLeague.Domain)

[SportsLeague.API](SportsLeague.API) → referencia a → [SportsLeague.DataAccess](SportsLeague.DataAccess)

[SportsLeague.DataAccess](SportsLeague.DataAccess) → referencia a → [SportsLeague.Domain](SportsLeague.Domain)

[SportsLeague.Domain](SportsLeague.Domain) NO referencia a ningún otro proyecto.<br><br/>

**Principio clave**: Domain es la capa central e independiente. Define contratos (interfaces) que las otras capas implementan. Si mañana quisiéramos cambiar SQL Server por PostgreSQL, solo tocaríamos DataAccess. Si cambiáramos el framework web, solo tocaríamos API. La lógica de negocio permanece intacta.
