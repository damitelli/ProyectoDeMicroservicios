# Proyecto De Microservicios
Consiste en el backend de 3 microservicios desarrollados en .Net (Catalog, Favorite List e Identity) corriendo por detrás de una API Gateway construida con Ocelot.


# Descripción General
- API Gateway
  Control de acceso a los microservicios.
  
- Microservicios
  - Catalog
    Contiene el catalogo de items. Permite hacer un CRUD de los mismos. Desde aquí se puede seleccionar un item como favorito. Esta acción envía un mensaje con los datos de usuario y de la selección, que por medio de Mass Transit y RabbitMq, es recibido por el microservicio de Favorite List. 

  - Favorite List
    Contiene los items seleccionados por los usuarios y las listas de favoritos pertenecientes a cada uno. La informarción no solo proviene de los mensajes sino que también es posible hacer un CRUD tanto de los items como de las listas, siempre y cuando se tengan los permisos correspondientes.

  - Identity
    Encargado de la autenticación, autorización, acceso y modificación de los datos de usuario.


# Patrones de diseño aplicado al microservicio
- Arquitectura de Microservicios
- Clean Architecture
- CQRS
- Mediator

# Stack tecnológico
- ASP.NET Core 7.0
- Entity Framework Core
- ASP.NET Core Identity
- PostgreSQL
- MongoDB
- JSON Web Token
- Automapper
- FluentValidation
- MediatR
- Mass Transit
- RabbitMq
- Nlog
- xUnit
- Ocelot
- Docker
- Docker-Compose

# Como iniciar el proyecto
Pensado para correr en Docker.

Una vez se tenga instalado Docker desktop, correr el siguiente comando en la carpeta contenedora de Docker Compose:

<pre><code>docker compose up</code></pre>

Una vez las imágenes hayan sido construidas y esten corriendo en Docker, es necesario llenar la base de datos para completa funcionalidad.

Se puede hacer desde el CLI con los comandos de Entity Framework Core. 

Primero debemos situarnos en la carpeta de Infrastructure de Identity y luego ejecutar:

<pre><code>dotnet ef migrations add NombreDeLaMigracion --startup-project ../publicapi</code></pre>
<pre><code>dotnet ef database update --startup-project ../publicapi</code></pre>

Los datos del super admin son los siguientes:

Usuario: superadmin
<br>
Contraseña: SA!123456


# Autores
- [Damitelli](https://github.com/damitelli)
- [ZC-72](https://github.com/zc-72)

# Licencia
Puedes leer más en el archivo [license.md](https://github.com/damitelli/ProyectoDeMicroservicios/blob/main/LICENSE)
