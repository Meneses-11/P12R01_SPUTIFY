🎵 Sputiffy
Aplicación web estilo Spotify-clone desarrollada en ASP.NET MVC 5 sobre .NET Framework 4.8, con Entity Framework 6 y SQL Server como motor de base de datos.

🚀 Tecnologías principales
ASP.NET MVC 5.2.9 (patrón MVC, vistas Razor .cshtml)

Entity Framework 6.0 (ORM para acceso a datos)

SQL Server (base de datos relacional)

Newtonsoft.Json 13.0.0 (serialización JSON)

System.Web.Optimization + WebGrease (bundling y minificación)

IIS Express (servidor de desarrollo en Visual Studio)

📂 Estructura del proyecto
Controllers/ → Controladores MVC para manejar lógica de negocio.

Models/ → Entidades y clases de acceso a datos (EF).

Views/ → Vistas Razor organizadas por módulo:

Usuarios, Roles, Albumes, Artistas, Canciones, Playlists, Favoritos, HistorialReproducciones, RecomendacionesUsuarios, PreferenciasUsuarios, etc.

Carpeta Shared/ para layouts y vistas comunes.

App_Start/ → Configuración de rutas, filtros y bundles.

Web.config → Configuración de dependencias, conexión a BD y runtime.

🗄️ Base de datos
Nombre: sputiffy

Tablas principales
Roles → Administración de permisos (Administrador, Usuario).

Usuarios → Datos de usuarios registrados.

Generos → Catálogo de géneros musicales.

Artistas → Información de artistas.

Albumes → Discografía asociada a artistas.

Canciones → Canciones con metadatos (duración, preview, URL, género, artista, álbum).

Playlists → Listas de reproducción creadas por usuarios.

PlaylistDetalle → Canciones dentro de playlists.

Favoritos → Canciones marcadas como favoritas por usuarios.

HistorialReproduccion → Registro de canciones reproducidas.

RecomendacionesUsuario → Sugerencias precalculadas para cada usuario.

PreferenciasUsuario → Configuración personalizada (género favorito, década preferida).

Índices
Optimización en consultas por año de lanzamiento, género, artista y usuario.

Datos iniciales
Roles: Administrador, Usuario.

Géneros: Pop, Rock, Balada, Electrónica, Regional Mexicano, Rap, Clásica, Jazz.

Usuario admin por defecto: admin (correo: admin, contraseña en texto plano).

⚠️ Nota de seguridad: actualmente las contraseñas se guardan en texto plano. Se recomienda implementar hashing seguro (ej. SHA256 + salt o ASP.NET Identity) antes de usar en producción.

▶️ Ejecución
Restaurar paquetes NuGet.

Configurar cadena de conexión en Web.config (por defecto usa integrated security=True).

Ejecutar script SQL para crear la BD sputiffy.

Levantar el proyecto en Visual Studio (IIS Express).

Acceder vía navegador: http://localhost:puerto/.

📌 Funcionalidades previstas
Registro y login de usuarios.

Gestión de playlists y favoritos.

Reproducción de canciones con historial.

Recomendaciones basadas en preferencias y escuchas previas.

Integración con APIs externas (ej. SpotifyAuth).