### ¿Qué vamos a construir?

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

**Capa de presentación**: SportsLeague.API

Es el punto de entrada de la aplicación. Recibe las [peticiones HTTP](APP_SportsLeague.md#HTTP_Request) del frontend (o de Swagger/Postman), las procesa y devuelve respuestas JSON.

- [Controllers](APP_SportsLeague.md#Controllers): reciben las peticiones y delegan al service correspondiente.<br/><br/>

- [DTOs](APP_SportsLeague.md#DTO): definen la estructura de datos que se envía y se recibe del frontend. Nunca se exponen las Entities directamente.<br/><br/>

- [Mappings](APP_SportsLeague.md#Object-to-Object-Mapping): configuración de AutoMapper para convertir entre DTOs y Entities.<br/><br/>

- [Middlewares](APP_SportsLeague.md#Middleware): manejo global de errores y logging.

- [Programs.cs](SportsLeague.API/Program.cs): configuración de servicios e inyección de dependencias.
