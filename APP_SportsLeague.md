https://github.com/Luffy-sudo/DisenoProg_L_APP_SportsLeague.git forked from https://github.com/cdiazserna/DisenoProg_L_APP_SportsLeague.git

# APP_SportsLeague

"ASP.NET Core Web API project with a clear layered structure (API, Domain, DataAccess)".

## High Level

#### Layers

The project follows a layered architecture, a common design pattern in software development (e.g, Clean Architecture or Onion Architecture) to promote separation of concerns, maintainability,and scalability. Based on the strcuture:

- [SportsLeague.API](SportsLeague.API/): _Presentation Layer_

  Handles HTTP requests, controllers, DTOs, and mappings. It exposes endpoints for clients (e.g., web/mobile apps) and translates between external requests and internal business logic.

- [SportsLeague.Domain](SportsLeague.Domain/): _Business Logic Layer_

  Contains core entities (e.g., Player, Team), services (e.g., PlayerService), enums, and interfaces. This layer encapsulates business rules and is independent of external concerns like databases or APIs.

- [SportsLeague.DataAccess](SportsLeague.DataAccess/): _Data Access Layer_

  Manages database interactions via Entity Framework (e.g., LeagueDbContext, repositories like PlayerRepository, migrations). It abstracts data persistence from the domain.

This separation allows changes in one layer (e.g., switching databases) without affecting others, improves testability, and follows SOLID principles. The [SportsLeague.slnx](SportsLeague.slnx/) file suggests a solution with multiple projects, reinforcing modularity.

# Tech Stack

## Backend

<h4 id="dotnet8">.NET 8</h4>
.NET is a free, cross-platform, open-source developer platform for building many kinds of applications. It can run programs written in multiple languages, with C# being the most popular.<br><br>

<h4 id="csharp">C#</h4>
C# is a modern, innovative, open-source, cross-platform object-orientes programming language and one of the top 5 programming languages on GitHub. <br><br>

<h4 id="EFCoreCodeFirst">EF Core Code First</h4>
The EF Core Code First approach is a development methodology where the application's C# classes and configurations are used to generate or evolve the database schema automatically. This approach prioritizes domain modeling in code over manual database design.<br><br>

<h4 id="SQL Server"> SQL Server</h4><br>

<h4 id="AutoMapper">AutoMapper</h4> 
The most widely adopted convention-based mapping tool. It uses profiles to define mapping configurations with minimal code.<br><br>

<h4 id="Swagger">Swagger</h4> 
Suite of tools for API developers. Swagger's open-source tooling usage can be broken up into different use cases: development, interaction with APIs, and documentation.<br><br>

<h4 id="Swashbuckle">Swashbuckle.AspNetCore</h4>: Is an open-source library that provides OpenAPI (formerly Swagger) tooling for APIs built with ASP.NET Core. It automatically generates interactive documentation and testing UI directly from your application's routes, controllers, and models.

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

## Instancia de DbContext

Una instancia de DbContext representa una sesión con la base de datos y se puede usar para consultar y guardar instancias de las entidades. DbContext es una combinación de los patrones de unidad de trabajo y repositorio.

## API Web

Una API Web es una interfaz de programación que permite la comunicación entre diferentes aplicaciones, servidores o servicios a través de internet utilizando protocolo HTTP. Actúa como un intermediario estandarizado para intercambiar datos (frecuentemente en formato JSON o XML) y funcionalidades, facilitando la integración de software sin conocer los detalles internos de cada sistema.

_Funcionamiento_: Se basa en peticiones HTTP (GET, POST, PUT, DELETE) hacia endpoints (URLs) específicas para consultar o manipular datos.

## API REST

Una API REST (Transferencia de Estado Representacional) es una interfaz de programación que permite la comunicación entre aplicaciones a través de HTTP, usando estándares web como JSON o XML para el intercambio de datos.

Creación de API Web con ASP.NET Core

ASP.NET Core (framework) admite la creación de API Web mediante controladores o mediante API mínimas. Los controladores de una API web son clases que se derivan de **ControllerBase** (A base class for an MVC controller without view support). Los controladores se activan y eliminan por solicitud.

<h2 id="Controllers">Controladores</h4>

Un controlador es una clase instanciable, normalmente pública, en la que se cumple **al menos una de las siguientes condiciones:**

- El nombre de la clase tiene el sufijo \*_Controller_.
- La clase se hereda de una clase cuyo nombre tiene el sufijo _Controller_.
- El atributo [Controller] se aplica a la clase.

Los controladores se usan para definir y agrupar un conjunto de acciones. Una acción (o método de acción) es un método en un controlador que controla las solicitudes. Los controladores agrupan lógicamente acciones similares. Esta agregación de acciones permite aplicar de forma colectiva conjuntos comunes de reglas, como el enrutamiento, el almacenamiento en caché y la autorización. Las solicitudes se asignan a acciones mediante el enrutamiento. Los controladores se activan y eliminan por solicitud.

En el patrón Model-View-Controller, un controlador se encarga del procesamiento inicial de la solicitud y la creación de instancias del modelo.

El controlador toma el resultado del procesamiento del modelo (si existe) y devuelve o bien la vista correcta y sus datos de vista asociados, o bien el resultado de la llamada API.

Por convención, las clases de controlador:

- Residen en la carpeta Controllers del nivel raíz del proyecto.
- Heredan de _Microsoft.AspNetCore.Mvc.Controller_

Un controlador en ASP.NET es una clase C# fundamental en la arquitectura MVC o API Web, encargada de gestionar las solicitudes HTTP entrantes del navegador o cliente. Actúa como intermediario: recibe la petición, procesa la lógica de negocio, interactúa con el modelo (datos) y devuelve una respuesta, usualmente una vista HTML o datos JSON.

**Aspectos clave de los controladores en ASP.NET**

- **Clases Públicas**: Generalmente heredan de _Controller_ (MVC) o _ControllerBase_ (API) y suelen llevar el sufijo "Controller" (ej: Homecontroller).

- **Acciones (Actions)**: Los métodos públicos dentro de un controlador se llaman "acciones", que responden a URLs específicas.

- **Manejo de Solicitudes (HTTP)**: Utilizan métodos como HttpGet o HttpPost para procesar peticiones.

- **Respuestas**: Pueden devolver vistas (ViewResult), datos (JsonResult), redirecciones (RedirectResult), entre otros.

- **Intermediario**: Su función es coordinar la entrada de datos del usuario, actualizar el modelo y decidir qué vista mostrar.<br/><br/>

<h4 id="DTO">Data Transfer Object (DTO)</h4>

Is a design pattern used in software development to move data efficiently between different layers or processes of an application. A DTO is a simple object that only contains data storage, accesors (getters and setters), and possibly serialization logic, but no business logic.</br><br/>

<h4 id="Object-to-Object-Mapping"> Object to Object Mapping (DTOs and Entitites)</h4>

Object mapping is the process of converting an object of one type (e.g., a database entity) to another type (e.g., a Data Transfer Object, used in API response). This helps decouple layers, improve performance, and enhance security.<br/><br/>

<h4 id="dotnet_solution">.NET Solution</h4>

A .NET solution (.sln) is a container or structural file used by IDEs like Visual Studio to organize one or more related code projects.<br/><br/>

##### _Key details about solutions_<br/></br>

- **Structure**: Solutions manage dependencies between projects, allowing them to share code.<br/><br/>

- **Contents**: A solution can hold multiple projects, such as class libraries, executable applications and test projects.<br/><br/>

- **Relationship to projects**: A solution is a higher-level container, whereas a project contains the actual source code compiled into a single assembly (DLL or EXE).<br><br/>

##### _Purpose and Benefits_<br/><br/>

- **Organizing Projects**: simplifies management of large-scale applications with multiple components.<br/><br/>

- **Building**: Allows building the entire collection of projects at once.<br/><br/>

<h4 id="HTTP_Request">HTTP Request</h4>

An <a href="https://www.ibm.com/docs/api/v1/content/SSJL4D_6.x%2Ffundamentals%2Fweb%2Fdfhtl21.html?parsebody=true&lang=en">HTTP Request</a> is made by a client, to a named host, which is located on a server. The aim of the request is to access a resource on the server.<br/>

To make the request, the client uses components of a URL (Uniform Resource Locator), which inclides the information needed to access the resource.<br/>

A correctly composed HTTP request contains the following elements: <br/>

1. A request line
2. A series of HTTP headers, or header fields.
3. A message body, if needed.<br/><br/>

<a href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-10.0"><h4 id = "Middleware">ASP.NET Core Middleware</h4></a>

Middleware is software that's assembled into an app pipeline to handle requests and responses. Each middleware:

- Choses whether to pass the request to the next middleware in the pipeline.
- Can perform work before and after the next middleware in the pipeline
