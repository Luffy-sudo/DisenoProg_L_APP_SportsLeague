#!meta

{"kernelInfo":{"defaultKernelName":"csharp","items":[{"name":"csharp","languageName":"csharp"},{"name":"fsharp","languageName":"F#","aliases":["f#","fs"]},{"name":"html","languageName":"HTML"},{"name":"http","languageName":"HTTP"},{"name":"javascript","languageName":"JavaScript","aliases":["js"]},{"name":"mermaid","languageName":"Mermaid"},{"name":"pwsh","languageName":"PowerShell","aliases":["powershell"]},{"name":"value"}]}}

#!markdown

https://github.com/Luffy-sudo/DisenoProg_L_APP_SportsLeague.git forked from https://github.com/cdiazserna/DisenoProg_L_APP_SportsLeague.git

The **git fetch** command downloads commits, files and refs from a remote repository into your local repository without updating your local working branch. This allows you to review the changes before integrating them into your work.

The **git pull** command downloads content and automatically integrates the new commits into your current local branch and working directory.

**APP_SportsLeague**

"ASP.NET Core Web API project with a clear layered structure (API, Domain, DataAccess)".

**High Level**

_Layers_

The project follows a layered architecture, a common design pattern in software development (e.g, Clean Architecture or Onion Architecture) to promote separation of concerns, maintainability,and scalability. Based on the strcuture:

- [SportsLeague.API](SportsLeague.API/): _Presentation Layer_

  Handles HTTP requests, controllers, DTOs, and mappings. It exposes endpoints for clients (e.g., web/mobile apps) and translates between external requests and internal business logic.

- [SportsLeague.Domain](SportsLeague.Domain/): _Business Logic Layer_

  Contains core entities (e.g., Player, Team), services (e.g., PlayerService), enums, and interfaces. This layer encapsulates business rules and is independent of external concerns like databases or APIs.

- [SportsLeague.DataAccess](SportsLeague.DataAccess/): _Data Access Layer_

  Manages database interactions via Entity Framework (e.g., LeagueDbContext, repositories like PlayerRepository, migrations). It abstracts data persistence from the domain.

This separation allows changes in one layer (e.g., switching databases) without affecting others, improves testability, and follows SOLID principles. The [SportsLeague.slnx](SportsLeague.slnx/) file suggests a solution with multiple projects, reinforcing modularity.

*Tech*: ASP.NET Core Web API, Entity Framework Core (Code-First), AutoMapper, dependency injection, ILogger.

**Data model / structures**

    - Entities: Team, Player, Referee, TournamentTeam (join entity). All inherit AuditBase (Id, CreatedAt, UpdatedAt).

    - Enums: PlayerPosition (and TournamentStatus used by Tournament).

    - Relations

        - Team 1:N Players (Players, TeamID)
        - TournamentTeam is a joint with unique compsite index per (TournamentID, TeamID).

    - Database constraints: unique indexes (e.g., Team.Name unique); composite unique index on Player (TeamID, Number) to enforce unique jersey numbers per team.

**Design patterns / Architecture choices**

- _Service layer_: business logic lives in services (e.g., PlayerService, TeamService) that call repositories and perform validations (existence, uniqueness).

- _Files_:

- [SportsLeague.Domain/Services](SportsLeague.Domain/Services/)
- [SportsLeague.Domain/Interfaces/Services/](SportsLeague.Domain/Interfaces/Services/)

- _DTOs + AutoMapper_: request / response DTOs separate API contract from domain entities; mapping configured in MappingProfile.

- _Files_:

- [SportsLeague.API/DTOs](SportsLeague.API/DTOs/)
- [MappingProfile.cs](SportsLeague.API/Mappings/MappingProfile.cs)

- _Dependency Injection_: configured in [Programs.cs](SportsLeague.API/Program.cs)
  - Logging: Uses ILogger in services for observability.

  - Error handling in controllers: throws / catches KeyNotFound Exception and InvalidOperationException to return 404/409.

**EF Core specifics**

- League DbContext configures entities and indexes in _OnModelCreating_ (ModelBuilder).

- _File_: [LeagueDbContext.cs](SportsLeague.DataAccess\Context\LeagueDbContext.cs)

- Migrations present (Code-First SQL generation) under [Migrations](SportsLeague.DataAccess/Migrations/)

**API Web**

Una API Web es una interfaz de programación que permite la comunicación entre diferentes aplicaciones, servidores o servicios a través de internet utilizando protocolo HTTP. Actúa como un intermediario estandarizado para intercambiar datos (frecuentemente en formato JSON o XML) y funcionalidades, facilitando la integración de software sin conocer los detalles internos de cada sistema.

_Funcionamiento_: Se basa en peticiones HTTP (GET, POST, PUT, DELETE) hacia endpoints (URLs) específicas para consultar o manipular datos.

**Creación de API Web con ASP.NET Core**

ASP.NET Core (framework) admite la creación de API Web mediante controladores o mediante API mínimas. Los controladores de una API web son clases que se derivan de **ControllerBase** (A base class for an MVC controller without view support). Los controladores se activan y eliminan por solicitud.

**Controladores**

Un controlador es una clase instanciable, normalmente pública, en la que se cumple **al menos una de las siguientes condiciones:**

- El nombre de la clase tiene el sufijo \*_Controller_.
- La clase se hereda de una clase cuyo nombre tiene el sufijo _Controller_.
- El atributo [Controller] se aplica a la clase.

Los controladores se usan para definir y agrupar un conjunto de acciones. Una acción (o método de acción) es un método en un controlador que controla las solicitudes. Los controladores agrupan lógicamente acciones similares. Esta agregación de acciones permite aplicar de forma colectiva conjuntos comunes de reglas, como el enrutamiento, el almacenamiento en caché y la autorización. Las solicitudes se asignan a acciones mediante el enrutamiento. Los controladores se activan y eliminan por solicitud.

Por convención, las clases de controlador:

- Residen en la carpeta Controllers del nivel raíz del proyecto.
- Heredan de _Microsoft.AspNetCore.Mvc.Controller_

Un controlador en ASP.NET es una clase C# fundamental en la arquitectura MVC o API Web, encargada de gestionar las solicitudes HTTP entrantes del navegador o cliente. Actúa como intermediario: recibe la petición, procesa la lógica de negocio, interactúa con el modelo (datos) y devuelve una respuesta, usualmente una vista HTML o datos JSON.

**Aspectos clave de los controladores en ASP.NET**

- **Clases Públicas**: Generalmente heredan de _Controller_ (MVC) o _ControllerBase_ (API) y suelen llevar el sufijo "Controller" (ej: Homecontroller).

- **Acciones (Actions)**: Los métodos públicos dentro de un controlador se llaman "acciones", que responden a URLs específicas.

- **Manejo de Solicitudes (HTTP)**: Utilizan métodos como HttpGet o HttpPost para procesar peticiones.

- **Respuestas**: Pueden devolver vistas (ViewResult), datos (JsonResult), redirecciones (RedirectResult), entre otros.

- **Intermediario**: Su función es coordinar la entrada de datos del usuario, actualizar el modelo y decidir qué vista mostrar.
