**Múltiples Foreign Keys que apuntan a la misma tabla**

		Tabla Team                    Tabla Match
	--------------------           ----------------------------------
	| Id (PK)		   |  <------  | Id (PK)                        |   
	| Name			   |  <------  | HomeTeamId (FK1) --> Team.Id   | -- Define el equipo local
	--------------------	       | AwayTeamId (FK2) --> Team.Id   | -- Define el equipo visitante
	                               | TournamentId (FK)              | -- Agrupa el partido en una liga/copa
	                               | RefereeId (FK)                 | -- Asigna un juez al encuentro
	                               | MatchDate                      |
	                               | Status                         |
	                               ---------------------------------

Match tiene 4 Foreign Keys: HomeTeamId, AwayTeamId, TournamentId, RefereeId. 

La tabla Match tiene dos columnas que apuntan a Team: HomeTeamId (equipo local) y AwayTeamId (equipo visitante). Esto genera dos relaciones 1:N independientes desde Team hacia Match.

Esta normalización asegura que si, por ejemplo, un equipo cambia de nombre en la tabla Team, el cambio se refleje automáticamente en todos sus partidos sin tener que editar la tabla Match registro por registro.


**El problema de los ciclos de cascada**

**Delete Behavior Enumeración**

Indica como se aplica una operación de eliminación a las entidades dependientes de una relación cuando se elimina la entidad de seguridad o se elimina la relación.


**Campos**

--------------------------------------------------------------------------------------------------------------------------------------------
|**Nombre**		**Valor**			**Description**                                                                                        
|                                                                                                                                          
| Cascade			   3			Elimina automáticamente las entidades dependientes cuando se elimina la relación con la entidad de       |								    seguridad y crea una restricción de clave externa en la base de datos con eliminaciones en cascada    	|									habilitadas.
-------------------------------------------------------------------------------------------------------------------------------------------
|
| Restrict		       1			Establece los valores de clave externa en **null** según corresponda cuando se realizan cambios en 		|									las entidades con seguimiento y crea una restricción de clave externa que no es en cascada en la base de |									datos.
---------------------------------------------------------------------------------------------------------------------------------------------

Como la tabla **Match** tiene múltiples FKs hacia la tabla **Team**, con DeleteBehavior.Cascade, SQL Server detecta múltiples caminos de cascada y rechaza la migración con un error. La solución es usar DeleteBehavior.Restrict en las FKs hacia **Team**. Esto significa que no se puede eliminar un **Team** si tiene partidos asociados. El usuario debe eliminar primero los partidos manualmente.

**Enum MatchStatus (SportsLeague.Domain/Enums/MatchStatus.cs)**

Las transiciones válidas de MatchStatus:

	Scheduled --> InProgress --> Finished
	|                |
	|                |
	 -----------> Suspended

**Entity Match (SportsLeague.Domain/Entities/Match.cs)**

Match tiene 4 Navigation Properties, una por cada FK. Esto permite acceder a match.HomeTeam.Name, match.AwayTeam.Name, match.Tournament.Name y match.Referee.LastName sin queries adicionales (usando .Include()).






